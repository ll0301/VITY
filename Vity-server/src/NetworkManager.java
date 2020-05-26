import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.JSONValue;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;


public class NetworkManager extends Thread {
	
	int port =9321;
	byte[] data = new byte[65535];
	public DatagramSocket socket;
	DatagramPacket packet;
	JSONParser jsonParser;
	InetAddress address;
	public void run() {
		
		try {
			// 상대방이 연결할수 있도록 udp 소켓생성
			jsonParser = new JSONParser();
			socket = new DatagramSocket(port);
			packet = new DatagramPacket(data, data.length);
			System.out.println("GameRoom접속 대기중 ...");
			//데이터전송받기
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
					String myPoint = ((String)r.get("myPoint").toString()); // 포인트
					//String pn = ((String)r.get("playerNumber").toString()); // 포인트
					String gr = ((String)r.get("group").toString()); // 포인트
					//int playerNumber = Integer.parseInt(pn);
					int group = Integer.parseInt(gr);
					boolean firstData = ((boolean)r.get("firstData"));
					boolean characterSet = ((boolean)r.get("characterSet"));
					boolean move = ((boolean)r.get("move"));
					boolean punch = ((boolean)r.get("punch"));
					boolean alive = ((boolean)r.get("alive"));
					boolean winner = ((boolean)r.get("winner"));
					boolean throwMode = ((boolean)r.get("throwMode"));
					boolean throwItem = ((boolean)r.get("throwItem"));
					boolean pickup = ((boolean)r.get("pickup"));
					boolean drop = ((boolean)r.get("drop"));
					//boolean receivePacket = ((boolean)r.get("receivePacket"));
					String px = ((String)r.get("pX").toString()); // 포인트
					String py = ((String)r.get("pY").toString()); // 포인트
					String pz = ((String)r.get("pZ").toString()); // 포인트
					String ry = ((String)r.get("rY").toString()); // 포인트
					String runX = ((String)r.get("runX").toString()); // 포인트
					String runZ = ((String)r.get("runZ").toString());
					Object udpObj = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/udpRoom.json"));
					JSONObject uob = (JSONObject) udpObj;
					JSONArray udpRoom = (JSONArray) uob.get("udpRoom");
					//String udpJson = JSONValue.toJSONString(uob);
					//System.out.println(uob);
					//System.out.println(udpJson);
					
					
					if(!characterSet) {
						//udp 룸 json 파일을 파싱한다. 
						for(int i = 0; i < udpRoom.size(); i++) {
							JSONObject tmp = (JSONObject) udpRoom.get(i);
							String ow = (String) tmp.get("owner");
							// 오브젝트안에 owner에 내가 속한방의 방장이름이 들어있으면 
							if(ow.equals(owner)) {
								JSONArray pList = (JSONArray)tmp.get("playerList");
								//맨처음 접속시 추가하기
								if(firstData) {
									JSONObject upo = new JSONObject();
									upo.put("name",name);
									//upo.put("address",packet.getAddress().toString());
									//upo.put("port",String.valueOf(packet.getPort()));
									upo.put("position",myPoint);
									upo.put("x",px);
									upo.put("y",py);
									upo.put("z",pz);
									upo.put("ry",ry);
									upo.put("runX",runX);
									upo.put("runZ",runZ);
									upo.put("punch",punch);
									upo.put("alive",alive);
									upo.put("move", move);
									upo.put("winner", winner);
									upo.put("drop", drop);
									upo.put("pickup", pickup);
									upo.put("throwItem", throwItem);
									upo.put("throwMode", throwMode);
									
									//upo.put("move",move);
									pList.add(upo);
									// 방정보를 저장하고있는 json 파일을 업데이트한다.  
									try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/udpRoom.json")) // json 파일에 문자를 작성한다. 
									{
										file.write(uob.toString()); // jsonobject에 들어있는 문자를 입력 
										file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
									}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
									
									// 방정보를 저장하고있는 json 파일을 업데이트한다.  
									try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/"+ow+".json")) // json 파일에 문자를 작성한다. 
									{
										file.write(pList.toString()); // jsonobject에 들어있는 문자를 입력 
										file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
									}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
									
									JSONArray nList = new JSONArray();
									// 방정보를 저장하고있는 json 파일을 업데이트한다.  
									try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/"+ow+"NPC"+".json")) // json 파일에 문자를 작성한다. 
									{
										file.write(nList.toString()); // jsonobject에 들어있는 문자를 입력 
										file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
									}catch(IOException e){e.printStackTrace();}// 파일에 추가하기										
									
									JSONArray iList = new JSONArray();
									// 방정보를 저장하고있는 json 파일을 업데이트한다.  
									try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/item/"+ow+"Item"+".json")) // json 파일에 문자를 작성한다. 
									{
										file.write(iList.toString()); // jsonobject에 들어있는 문자를 입력 
										file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
									}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
									
									// 플레이리스트에 추가된 사람들에게 정보를 업데이트한다.
									//System.out.println(name+" udp connect");
									
								}// 접속한 사람의 정보를 추가한다.
									
								if(group==2) {
									JSONObject setGame = new JSONObject();													
									setGame.put("setGame",pList);
									
									if(pList.size()==2) {
									setGame.put("listSize","true");	
									}else {
										setGame.put("listSize","false");
									}
									try {
										String setGameJson = JSONValue.toJSONString(setGame);
										byte[] byteArr = setGameJson.getBytes("UTF-8");
										DatagramPacket sendPacket = 
												new DatagramPacket(
														byteArr,
														byteArr.length,
														packet.getAddress(),
														packet.getPort());
										socket.send(sendPacket);	
									}catch(Exception e) {
										System.out.println(e.getMessage());
									}	
									
								}
								if(group==4) {
									JSONObject setGame = new JSONObject();													
									setGame.put("setGame",pList);
									
									if(pList.size()==4) {
									setGame.put("listSize","true");	
									}else {
										setGame.put("listSize","false");
									}
									try {
										String setGameJson = JSONValue.toJSONString(setGame);
										byte[] byteArr = setGameJson.getBytes("UTF-8");
										DatagramPacket sendPacket = 
												new DatagramPacket(
														byteArr,
														byteArr.length,
														packet.getAddress(),
														packet.getPort());
										socket.send(sendPacket);	
									}catch(Exception e) {
										System.out.println(e.getMessage());
									}
								}
							}
						}
					}
				}catch(ParseException e) {
					System.out.println(e.getMessage());
				}
				//socket.close();
			}
			
		}catch(Exception e) {
			System.out.println(e.getMessage());
		}
		socket.close();
	}
	
}