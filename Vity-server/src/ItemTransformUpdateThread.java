import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

public class ItemTransformUpdateThread extends Thread {
	
	int port =9888;
	byte[] data = new byte[65535];
	DatagramSocket socket;
	DatagramPacket packet;
	JSONParser jsonParser;
	
	public void run() {
		try {
			
			jsonParser = new JSONParser();
			socket = new DatagramSocket(port);
			packet = new DatagramPacket(data, data.length);
			while(true) {
				socket.receive(packet);	
				String str = new String (packet.getData(), packet.getOffset(), packet.getLength(),"Utf-8");
				
				try {
					JSONObject obj = (JSONObject) jsonParser.parse(str);
					JSONObject r = (JSONObject) obj.get("itemData");
					String name = ((String)r.get("name").toString());
					boolean isOwner = ((boolean)r.get("isOwner"));
					boolean firstData = ((boolean)r.get("firstData"));
					boolean pickUp = ((boolean)r.get("pickUp"));
					boolean drop = ((boolean)r.get("drop"));
					String pickupOwner = ((String)r.get("pickupOwner").toString());
					String owner = ((String)r.get("owner").toString());
					String px = ((String)r.get("pX").toString()); // ?�인??
					String py = ((String)r.get("pY").toString()); // ?�인??
					String pz = ((String)r.get("pZ").toString()); // ?�인??
					String ry = ((String)r.get("rY").toString()); // ?�인??
					Object udpObj = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/item/"+owner+"Item"+".json"));
					JSONArray iList = (JSONArray) udpObj;
					
					
						if(isOwner) {
							if(firstData) {
								JSONObject itemInfo = new JSONObject();
								itemInfo.put("name", name);
								itemInfo.put("pickupOwner", pickupOwner);
								itemInfo.put("x", px);
								itemInfo.put("y", py);
								itemInfo.put("z", pz);
								itemInfo.put("ry", ry);
								itemInfo.put("pickUp", pickUp);
								itemInfo.put("drop", drop);
								iList.add(itemInfo);
								
								try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/item/"+owner+"Item"+".json")) // json 파일에 문자를 작성한다. 
								{
									file.write(iList.toString()); // jsonobject에 들어있는 문자를 입력 
									file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
								}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
							//break;
							}
						}
					
					
					
				}catch(Exception e){System.out.println(e.getMessage());}
			
			}
			}catch(Exception e){System.out.println(e.getMessage());
		}
		socket.close();
	}
	
}
