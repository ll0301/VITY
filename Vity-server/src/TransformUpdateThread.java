import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

public class TransformUpdateThread extends Thread{

	int port =9510;
	byte[] data = new byte[65535];
	DatagramSocket socket;
	DatagramPacket packet;
	JSONParser jsonParser;
	InetAddress address;
	
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
					JSONObject r = (JSONObject) obj.get("roomData");
					String name = ((String)r.get("username").toString()); // ?��??�임
					String owner = ((String)r.get("owner").toString()); // 방장?�임
					boolean move = ((boolean)r.get("move"));
					String px = ((String)r.get("pX").toString()); // ?�인??
					String py = ((String)r.get("pY").toString()); // ?�인??
					String pz = ((String)r.get("pZ").toString()); // ?�인??
					String ry = ((String)r.get("rY").toString()); // ?�인??
					String runX = ((String)r.get("runX").toString()); // ?�인??
					String runZ = ((String)r.get("runZ").toString());
					boolean punch = ((boolean)r.get("punch"));
					boolean alive = ((boolean)r.get("alive"));
					boolean winner = ((boolean)r.get("winner"));
					boolean throwMode = ((boolean)r.get("throwMode"));
					boolean throwItem = ((boolean)r.get("throwItem"));
					boolean pickup = ((boolean)r.get("pickup"));
					boolean drop = ((boolean)r.get("drop"));
					Object udpObj2 = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/"+owner+".json"));
					JSONArray pList = (JSONArray) udpObj2;
					
					//if(move) {
						if(!winner) {
							for(int j = 0; j < pList.size(); j++) {
								JSONObject tmp2 = (JSONObject) pList.get(j);
								String pname = (String) tmp2.get("name");
								if(name.equals(pname)) {
									tmp2.replace("x", px);
									tmp2.replace("y", py);
									tmp2.replace("z", pz);
									tmp2.replace("ry", ry);
									tmp2.replace("runX",runX);
									tmp2.replace("runZ",runZ);
									tmp2.replace("punch", punch);
									tmp2.replace("alive", alive);
									tmp2.replace("move", move);
									tmp2.replace("winner", winner);
									tmp2.replace("drop", drop);
									tmp2.replace("pickup", pickup);
									tmp2.replace("throwItem", throwItem);
									tmp2.replace("throwMode", throwMode);
									//System.out.println(tmp2);
									//System.out.println(str);
									// 방정보�? ?�?�하고있??json ?�일???�데?�트?�다.  
									try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/"+owner+".json")) // json ?�일??문자�??�성?�다. 
									{
										
										file.write(pList.toString()); // jsonobject???�어?�는 문자�??�력 
										file.flush();//stream???�아?�는 ?�이?��? 강제�??�보?�는 ??��???�다. 
									}catch(IOException e){e.printStackTrace();}// ?�일??추�??�기	
								}
								}
						}
					
						//}
					
					
						if(winner) {
							
							JSONArray pl = new JSONArray();
							
							try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/"+owner+".json")) // json ?�일??문자�??�성?�다. 
							{
								file.write(pl.toString());
								file.flush();
							}catch(IOException e){e.printStackTrace();}
							
							JSONArray nl = new JSONArray();
							try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/"+owner+"NPC"+".json")) // json ?�일??문자�??�성?�다. 
							{
								file.write(nl.toString()); // jsonobject???�어?�는 문자�??�력 
								file.flush();//stream???�아?�는 ?�이?��? 강제�??�보?�는 ??��???�다. 
							}catch(IOException e){e.printStackTrace();}// ?�일??추�??�기	
						}
					
				}catch(Exception e){
					System.out.println(e.getMessage()+"1");
				}
			}		
		}catch(Exception e){
			System.out.println(e.getMessage()+"2");
		}
		socket.close();
	}
}

