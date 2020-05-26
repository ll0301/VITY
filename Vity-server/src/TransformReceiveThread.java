import java.io.FileReader;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.JSONValue;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

public class TransformReceiveThread extends Thread {

	int port =9500;
	byte[] data = new byte[65535];
	DatagramSocket socket;
	DatagramPacket packet;
	JSONParser jsonParser;
	InetAddress address;
	
	public void run() {

		 try{
				// 상대방이 연결할수 있도록 udp 소켓생성
				jsonParser = new JSONParser();
				socket = new DatagramSocket(port);
				packet = new DatagramPacket(data, data.length);
				System.out.println("!!GameWorld 접속 대기중 ...");
			 
			 while(true) {
				 socket.receive(packet);	
					String str = new String (packet.getData(), packet.getOffset(), packet.getLength(),"Utf-8");
					//System.out.println(str);
						//Thread thread = new MultiGameThread(socket,packet,name);
						//thread.start();
					try {
						JSONObject obj = (JSONObject) jsonParser.parse(str);
						JSONObject r = (JSONObject) obj.get("roomData");
						String name = ((String)r.get("username").toString()); // 유저네임
						String owner = ((String)r.get("owner").toString()); // 방장네임
						//System.out.println("owner : "+owner+" | player : "+ name);
						Object udpObj2 = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/"+owner+".json"));
						JSONArray pList = (JSONArray) udpObj2;
						//boolean send = false;
						for(int j = 0; j < pList.size(); j++) {
							JSONObject tmp2 = (JSONObject) pList.get(j);
							String pname = (String) tmp2.get("name");
							if(!name.equals(pname)) {
								//send=true;
								String positionJson = JSONValue.toJSONString(tmp2);
								//System.out.println(name+" : "+positionJson);
								try {
									byte[] bytePos = positionJson.getBytes("UTF-8");
									DatagramPacket sendPacket = 
												new DatagramPacket(
														bytePos,
														bytePos.length,
														packet.getAddress(),
														packet.getPort());
									socket.send(sendPacket);	
								}catch(Exception e) {
									System.out.println(e.getMessage());
								}	
							}
						}
						//socket.close();
						
					}catch(ParseException e) {
						System.out.println(e.getMessage());
					}	 
	         }
			 
	  } catch(Exception e){
	         System.out.println(e.getMessage());
	  }
		socket.close();
	}
}
