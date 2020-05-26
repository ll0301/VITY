import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

public class NPCTransformUpdateThread extends Thread {
	
	int port =9910;
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
					JSONObject r = (JSONObject) obj.get("npcData");
					String name = ((String)r.get("name").toString());
					String owner = ((String)r.get("owner").toString());
					boolean isOwner = ((boolean)r.get("isOwner"));
					boolean firstData = ((boolean)r.get("firstData"));
					boolean death = ((boolean)r.get("death"));
					boolean idle = ((boolean)r.get("idle"));
					String px = ((String)r.get("pX").toString()); // ?�인??
					String py = ((String)r.get("pY").toString()); // ?�인??
					String pz = ((String)r.get("pZ").toString()); // ?�인??
					String ry = ((String)r.get("rY").toString()); // ?�인??
					Object udpObj2 = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/"+owner+"NPC"+".json"));
					JSONArray nList = (JSONArray) udpObj2;

					
					if(isOwner) {
						if(firstData) {
							JSONObject npcInfo = new JSONObject();
							npcInfo.put("name", name);
							npcInfo.put("x", px);
							npcInfo.put("y", py);
							npcInfo.put("z", pz);
							npcInfo.put("ry", ry);
							npcInfo.put("death", death);
							npcInfo.put("idle", idle);
							nList.add(npcInfo);
							
							try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/"+owner+"NPC"+".json")) // json ?�일??문자�??�성?�다. 
							{
								file.write(nList.toString()); 
								file.flush();
							}catch(IOException e){e.printStackTrace();}
						
							
						}else {

							for(int j = 0; j < nList.size(); j++) {
								JSONObject tmp2 = (JSONObject) nList.get(j);
								String pname = (String) tmp2.get("name");
								if(name.equals(pname)) {
									tmp2.replace("x", px);
									tmp2.replace("y", py);
									tmp2.replace("z", pz);
									tmp2.replace("ry", ry);
									tmp2.replace("death", death);
									tmp2.replace("idle", idle);
									//System.out.println(tmp2);
									//System.out.println(str);
									// 방정보�? ?�?�하고있??json ?�일???�데?�트?�다.  
									try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/"+owner+"NPC"+".json")) // json ?�일??문자�??�성?�다. 
									{
										file.write(nList.toString()); // jsonobject???�어?�는 문자�??�력 
										file.flush();//stream???�아?�는 ?�이?��? 강제�??�보?�는 ??��???�다. 
									}catch(IOException e){e.printStackTrace();}// ?�일??추�??�기	
								}
								}
						}
						}
								
				}catch(Exception e){System.out.println(e.getMessage());}
			
			}
			}catch(Exception e){System.out.println(e.getMessage());
		}
		socket.close();
	}
	
	
}
