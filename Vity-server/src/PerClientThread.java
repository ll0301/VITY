
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.ArrayList;
import java.util.Collections;

import java.util.List;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.JSONValue;
import org.json.simple.parser.JSONParser;


//다중접속 구현해줄 쓰레드 
public class PerClientThread extends Thread {
	
	// printwriter = 기본 데이터형이나 객체를 쓰기 위한 클래스이다. 
	// reader가 없는 오직 출력을 위한 객체이다.  
	// Collections.synchronizedxxx() 메소드는
	// 비동기화된 메소드를 동기화된 메소드로 래핑한다. 
	// 파라미터로 비동기화된 컬렉션을 대입하면 동기화된 컬렉션을 린턴해준다. 
	// Vector와 Hash table은 동기화된 메소드로 구성되어있지만
	// ArrayList, HashSet, HashMap은 동기화된 메소드로 구성되어있지않아서
	// 멀티 스레드 환경에서 안전하지 않다.
	static List<PrintWriter> list = Collections.synchronizedList(new ArrayList<PrintWriter>()); 
	//소켓생성
	Socket socket;
	// printwriter 생성 
	PrintWriter pwriter;

	// 서버 메인에서의 소켓값을 받아온다.  
	public PerClientThread(Socket socket) {
		this.socket = socket;
		


		try {
			BufferedWriter writer = new BufferedWriter(
					new OutputStreamWriter(socket.getOutputStream(),"Unicode"));

			// getoutputstream() 쓰기메소드  getinputstream() 읽기메소드
			// 위의 메소드를 통해 메세지나 데이터를 실질적으로 통신한다. 
			// 소켓으로 통신한 메세지나 데이터를 출력해줄 writer를
			// 멀티접속으로 구현하기위해 리스트에 추가한다. 
			
			// pwriter 는 현재 클라이언트 
			pwriter = new PrintWriter(writer);
			list.add(pwriter);	
			
			//System.out.println("사이즈로 찾음 :" + list.get(size));
			//System.out.println("list size :" + size );
			System.out.println("tcp list : "+list);
			
			// list에 저장된 현재 클라이언트의 인덱스 번호를 받아온다. 
			//System.out.println("writer in list : " + list.indexOf(pwriter));
			//System.out.println("writer : " + pwriter);
			//System.out.println("bwriter : " + writer);
			
			}
		catch (Exception e) {
			System.out.println(e.getMessage());
		}
	}
	
	// Thread에서 해야하는일 
	// 동시작업이 필요한 것을 구현한다. 
	public void run() {
		ConnectUser();
	}
	
	// 플레이어 서버접속 시에 
	public void ConnectUser() {
		//서버접속 
				// 리스트에 담긴 순서를 저장함 
				//int id = size;
				String name = null;
				//int rHash;
				boolean roomCount = false;
				boolean connect = false;
				boolean ready = false;
				boolean gameStart = false;
				//String playerFilePath = "/home/ubuntu/Vity-server/player.json";
				String roomFilePath = "/home/ubuntu/Vity-server/roomList.json";
				try {
					JSONParser jsonParser = new JSONParser();
					// bufferedreader 클래스는 문자입력 스트림으로부터 문자들을 읽어드림 
					// 버퍼링을 함으로써 문자,문자배열, 문자열 라인등을 효율적으로 읽어드릴수 있도록 해준다.
					BufferedReader reader = new BufferedReader
							(new InputStreamReader(socket.getInputStream(),"Unicode"));
					// socket.getinputstream()으로 클라이언트로부터 통신해온 정보를 
					// bufferedreader로 읽어드린다. 
					//로그인할때 넘어온 json형식의 회원정보를 파싱한다.
						name = reader.readLine(); // 접속한 사용자의 이름지정 
						//sendAll(name + " is Joined"); // 클라이언트에 접속표시 
						System.out.println("user : "+ name + " tcp connect"); // 서버콘솔에 클라이언트 접속표시
						// 클라이언트에서 넘어오는 메세지를 가지고 상황을 구현한다. 

						//System.out.println("message" + msgJson);
						//클라이언 메세지를 한줄 파싱할때
						
						while(true) {

							//readline()  파일내용 한줄씩 읽어드린다. 
							String str = reader.readLine();
							//System.out.println(str);
							if(str == null) {
								break;
							}
							try {
								// 클라이언트가 보내는 유저의 상태 
									JSONObject jObj = (JSONObject) jsonParser.parse(str);
									JSONObject p = (JSONObject) jObj.get("playerData"); 
									String uname = ((String)p.get("username").toString()); // 유저네임 
									boolean refreshOn =((boolean)p.get("refreshOn")); // 새로고침을 실행했나요? 
									boolean inRoom =  ((boolean)p.get("inRoom")); // 게임방에 있는 상태인가요? 
									boolean roomOwner = ((boolean)p.get("roomOwner")); // 회원이 보낸 정보	
									boolean panelOff = ((boolean)p.get("panelOff")); // 게임방장일때 방을 나갔나요? 
									boolean groupCheck = ((boolean)p.get("groupCheck"));
									boolean privateRoom = ((boolean)p.get("privateRoom")); // 게임방이 비밀방인가요 아닌가요? 
									boolean readyCheck = ((boolean)p.get("readyCheck"));//레디 했는지 여부 
									boolean startGame = ((boolean)p.get("startGame"));// 게임 화면으로 이동했는지 여부 
									String roomPasswd = ((String)p.get("roomPasswd").toString()); // 게임방의 비밀번호 
									String pc = ((String)p.get("playerCount").toString());
									int playerCount = Integer.parseInt(pc);
									String ownerName =((String)p.get("ownerName").toString());
									String getMessage = ((String)p.get("message").toString());
									
								// 접속한 유저가 저장되는 json 파일 파싱하기  
								//Object obj = jsonParser.parse(new FileReader(playerFilePath));
								// 파싱해온 정보를 json object에 넣기 
								//JSONObject job = (JSONObject) obj; 
								//JSONObject connectPlayer = (JSONObject) job.get("playerList");
								//connectPlayer.put(uname,p); 
								//System.out.println("접속시 job :" + p);
								//job.put("playerList", connectPlayer);
							
								//udp 게임방 
								Object udpObj = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/udpRoom.json"));
								JSONObject uob = (JSONObject) udpObj;
								JSONArray udpRoom = (JSONArray) uob.get("udpRoom");
								
								//게임룸 정보가 저장되는 JSON 파일 파싱해서 플레이어에게 보여준다. 
								Object robj = jsonParser.parse(new FileReader(roomFilePath));
								JSONObject rob = (JSONObject) robj;
								JSONArray createdRoom = (JSONArray) rob.get("roomList");
								String roomJson = JSONValue.toJSONString(rob);
								//sSystem.out.println(roomJson);
								
								
								//게임오버 
								if(getMessage.equals("gameoverMsg")) 
								{
									//System.out.println("게임오버");
									//System.out.println("name"+name);
									gameStart = false;
									for(int o = 0; o < udpRoom.size(); o++) {
										JSONObject utmp = (JSONObject)udpRoom.get(o);
										//JSONArray ptmp = (JSONArray) utmp.get("playerList");
										String uow = (String)utmp.get("owner");
										if(uow.equals(name)){
											utmp.remove("playerList");
											JSONArray pl = new JSONArray();
											utmp.put("playerList",pl);
											// 방정보를 저장하고있는 json 파일을 업데이트한다.  
											try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/udpRoom.json")) // json 파일에 문자를 작성한다. 
											{
												file.write(uob.toString()); // jsonobject에 들어있는 문자를 입력 
												file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
											}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
										}
									}
									
								}
								
								// ----------------------접속 후에 전달받는 메세지 
								if(connect) {
									// ****클라이언트가 로비에있을때 
									if(!inRoom && !roomOwner) {
										
										//새로고침버튼을 누를때만 실행됨 
										if(refreshOn) 
										{
											//System.out.println("로비에서 새로고침버튼 누름!");
											//JSONObject user = (JSONObject) connectPlayer.get(name);
											//System.out.println("새로고침한 클라이언트 : "+user);
											//user.replace("refreshOn", false); 
											//System.out.println("새로고침 다시 false : " +user);
											//System.out.println("connectPlayer : "+connectPlayer);
											//System.out.println("job : " + job);
											list.get(list.indexOf(pwriter)).println(roomJson); //list에서 현재 클라이언트의 인덱스번호로 클라이언트를 구분한다.
											list.get(list.indexOf(pwriter)).flush();	
										}
										

										//방을 나오게 되면 . 삭제 
										else if(panelOff) 
										{
											
											//JSONObject user2 = (JSONObject) connectPlayer.get(uname);										
											//user2.replace("panelOff", false); 
											
											// 룸정보가 담긴 어레이리스트 확인 
												for(int i = 0; i<createdRoom.size(); i++) {
													JSONObject tmp = (JSONObject)createdRoom.get(i); //인덱스 번호로 접근 
													
													String ow = (String)tmp.get("owner");
													
													// 방장에 디스커넥트하는 클라이언트의 이름이 있으면  
													if(ow.equals(uname)) {
														//System.out.println(uname+"님이 만든방이 있습니다.");
														JSONArray pList = (JSONArray) tmp.get("playerList");
														
														// 디스커넥트하는 클라이언트가 저장되어있는 게임방 플레이어리스트에 접근 
														for(int j = 0; j<pList.size(); j++) {
															JSONObject tmp2 = (JSONObject) pList.get(j);
															String id = (String) tmp2.get("id");
															if(id.equals(uname)) {
																// 게임방 참여자 1명일때 
																if(pList.size()==1) {
																	pList.remove(j);
																	// 해당방을 지운다. 
																	
																	JSONObject removeId = new JSONObject();
																	removeId.put("roomOwner", uname);
																	JSONObject removeRoom = new JSONObject();
																	removeRoom.put("removeRoom", removeId);
																	String removeRoomJson = JSONValue.toJSONString(removeRoom);
																	sendAll(removeRoomJson);
																	
																	createdRoom.remove(tmp); 
																	
																	for(int u = 0; u < udpRoom.size(); u++) {
																		JSONObject utmp = (JSONObject)udpRoom.get(u);
																		String uow = (String)utmp.get("owner");
																		if(uow.equals(name)) {
																			udpRoom.remove(utmp);
																		}
																	}		
																	
																}
																
																// 방에 사람이 있을때
																else if (pList.size()>1)
																{  
																	JSONObject tmp3 = (JSONObject) pList.get(j+1);
																	//System.out.println("게임방에있는 다른유저 : " + tmp3);
																	String id2 = (String) tmp3.get("id");
																	//System.out.println("디스커넥트 유저가 사라진 리스트" + pList);									
																	tmp.replace("owner", id2);
																	tmp3.replace("ow", id2);
																	//System.out.println("방장이 변경된 게임방 : "+tmp);
																	if (pList.size()>2) {
																		JSONObject tmp4 = (JSONObject) pList.get(j+2);
																		tmp4.replace("ow", id2);
																	}
																	else if (pList.size()>3) {
																		JSONObject tmp5 = (JSONObject) pList.get(j+3);
																		tmp5.replace("ow", id2);
																	}
																	//System.out.println("게임방리스트 : " + createdRoom);	
																	for(int o = 0; o < udpRoom.size(); o++) {
																		JSONObject utmp = (JSONObject)udpRoom.get(o);
																		String uow = (String)utmp.get("owner");
																		if(uow.equals(name)) {
																			utmp.replace("owner", id2);
																		}
																	}
																	pList.remove(j);
																	
																	JSONObject changeId = new JSONObject();
																	changeId.put("currentOwner", uname);
																	changeId.put("changeOwner", id2);
																	JSONObject changeOwner = new JSONObject();
																	changeOwner.put("changeOwner", changeId);
																	String changeOwnerJson = JSONValue.toJSONString(changeOwner);
																	sendAll(changeOwnerJson);
																	//System.out.println(changeOwnerJson);
											
																}
																
																// 방에 사람이 있을때
																
																
															}//게임방 플레이어 리스트에서 디스커넥트하는 클라이언트의 이름을 지운다. 
														}// 디스커넥트하는 클라이언트가 저장되어있는 게임방 플레이어리스트에 접근 		
													}// 방장에 디스커넥트하는 클라이언트의 이름이 있으면	
													

													//방문자가 방을 나갈때 
													if(ow.equals(ownerName) && !ow.equals(uname)) {
														JSONArray pList = (JSONArray) tmp.get("playerList");
														// 디스커넥트하는 클라이언트가 저장되어있는 게임방 플레이어리스트에 접근 
														for(int j = 0; j<pList.size(); j++) {
															JSONObject tmp2 = (JSONObject) pList.get(j);
															
															String id = (String) tmp2.get("id");
															if(id.equals(uname)) {
																	pList.remove(j);
																	//System.out.println("ow" + ow);
																	//System.out.println("ownerNmae"+ownerName);
																	//System.out.println("pList" + pList);
																	//System.out.println("방문자가 방에서 나갈때");
																	
																	JSONObject removeId = new JSONObject();
																	removeId.put("id", uname);
																	removeId.put("message", "");
																	removeId.put("ow", ow);
																	JSONObject removePlayer = new JSONObject();
																	removePlayer.put("removePlayer", removeId);
																	String removePlayerJson = JSONValue.toJSONString(removePlayer);
																	sendAll(removePlayerJson);
																	//System.out.println(addPlayerJson);
																	
																	
																}
															}
													}
												
													//방문자일때
													
												}// 룸정보가 담긴 어레이리스트 확인 
											//1***********************게임방에있었다면 플레이리스트에서 지우기 
												roomCount = false;
												ready = false;
										}
										
										//누르지않으면  메세지 
										else if (!refreshOn && !panelOff )
										{
											String msgJson = JSONValue.toJSONString(p);
											//로비에있는 채팅방에 글을 올린다. 
											sendAll(msgJson);
											//System.out.println("message" + msgJson);
										
										}
										
										
										try (FileWriter file = new FileWriter(roomFilePath)) // json 파일에 문자를 작성한다. 
										{
											file.write(rob.toString()); // jsonobject에 들어있는 문자를 입력 
											file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
										}catch(IOException e){e.printStackTrace();}// 파일에 추가하기

										// 방정보를 저장하고있는 json 파일을 업데이트한다.  
										try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/udpRoom.json")) // json 파일에 문자를 작성한다. 
										{
											file.write(uob.toString()); // jsonobject에 들어있는 문자를 입력 
											file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
										}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
						
								
										
									}// ****클라이언트가 로비에있을때 
									
									// 클라이언트가 방을 생성했을 때!!! 
									if(inRoom && roomOwner) {
										
										// roomlist에서 index별로 값 구분할 수 있게
										String rname = ((String)p.get("roomName").toString()); 
										//System.out.println("rob"+ rob); --> 룸리스트 저장 
										//System.out.println("roomArray"+createdRoom);  --> 룸 어레이 추가 
										//System.out.println(uname+"님이 방을 생성합니다.");
										//System.out.println("username:"+uname+" | roomName:"+rname);
										
										if(roomCount) {
											// 메세지전송 
											if (!refreshOn&&!getMessage.equals("")&&!getMessage.equals("readyonoffvity")&&!getMessage.equals("kickoutplayer")) {
												String msgJson = JSONValue.toJSONString(p);
												//로비에있는 채팅방에 글을 올린다. 
												sendAll(msgJson);
												//System.out.println("방장 메세지전송");
											}
											
											else if(!refreshOn&&getMessage.equals("readyonoffvity")&&!getMessage.equals("kickoutplayer")) {
												for(int i = 0; i<createdRoom.size(); i++) 
												{
													JSONObject tmp = (JSONObject)createdRoom.get(i); //인덱스 번호로 접근 
													String ow = (String)tmp.get("owner");
													//현재 방에있는 오너를 찾는다. 
													if(ow.equals(ownerName))
													{
															JSONArray pList = (JSONArray) tmp.get("playerList");
															for(int j = 0; j<pList.size(); j++) {
																JSONObject tmp2 = (JSONObject) pList.get(j);
																String id = (String) tmp2.get("id");
																
																//나의 아이디를 찾는다. 
																if(id.equals(name)) {
																	
																	tmp2.replace("ready", readyCheck);
																	
																	if(!readyCheck) {
																		JSONObject readyOffId = new JSONObject();
																		readyOffId.put("id", name);
																		readyOffId.put("ready", readyCheck);
																		readyOffId.put("ow", ow);
																		
																		JSONObject readyOffPlayer = new JSONObject();
																		readyOffPlayer.put("readyOffPlayer", readyOffId);
																		String readyOffPlayerJson = JSONValue.toJSONString(readyOffPlayer);
																		sendAll(readyOffPlayerJson);

																		//System.out.println("방장 레디취소");
																	}
																	
																	else if(readyCheck) {
																		JSONObject readyId = new JSONObject();
																		readyId.put("id", name);
																		readyId.put("ready", readyCheck);
																		readyId.put("ow", ow);
																		
																		JSONObject readyPlayer = new JSONObject();
																		readyPlayer.put("readyPlayer", readyId);
																		String readyPlayerJson = JSONValue.toJSONString(readyPlayer);
																		sendAll(readyPlayerJson);

																		//System.out.println("방장 레디");
																	}
																}
															}
													}
												}
											
											}
											// 강제퇴장 명령어 
											else if (!refreshOn&&!getMessage.equals("readyonoffvity")&&getMessage.equals("kickoutplayer")) {
												
												//System.out.println("강퇴할놈 : " + roomPasswd);
												JSONObject kickOffId = new JSONObject();
												kickOffId.put("id", roomPasswd);
												kickOffId.put("ow", name);
							
												JSONObject kickOffPlayer = new JSONObject();
												kickOffPlayer.put("kickOffPlayer", kickOffId);
												String kickOffPlayerJson = JSONValue.toJSONString(kickOffPlayer);
												sendAll(kickOffPlayerJson);
												
											}
												//게임시작을 알림 
												if(startGame) {
													if(!refreshOn && !gameStart) {
														JSONObject startId = new JSONObject();
														startId.put("startGame", startGame);
														startId.put("ow", uname);
														
														JSONObject startPlayer = new JSONObject();
														startPlayer.put("startPlayer", startId);
														String startPlayerJson = JSONValue.toJSONString(startPlayer);
														sendAll(startPlayerJson);
														gameStart = true;
													}
												}
												
												if(refreshOn) 
												{
													//System.out.println("방에서 새로고침버튼 누름!");
													//JSONObject user = (JSONObject) connectPlayer.get(name);
													//System.out.println("새로고침한 클라이언트 : "+user);
													//user.replace("refreshOn", false); 
													//System.out.println("새로고침 다시 false : " +user);
													//System.out.println("connectPlayer : "+connectPlayer);
													list.get(list.indexOf(pwriter)).println(roomJson); //list에서 현재 클라이언트의 인덱스번호로 클라이언트를 구분한다.
													list.get(list.indexOf(pwriter)).flush();
													//System.out.println("방장 새로고침");
												}
												// 방정보를 저장하고있는 json 파일을 업데이트한다.  
												try (FileWriter file = new FileWriter(roomFilePath)) // json 파일에 문자를 작성한다. 
												{
													file.write(rob.toString()); // jsonobject에 들어있는 문자를 입력 
													file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
												}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
													
										}
										
										//**********************************방 생성하고 1번만 실행 
										else if(!roomCount) {
											// 방정보를 담을 오브젝트를 생성한다.  
											//JSONObject roomHash = new JSONObject();
											JSONObject roomInfo = new JSONObject();	
											roomInfo.put("roomName", rname);
											roomInfo.put("owner", uname);
											roomInfo.put("private", privateRoom);
											roomInfo.put("groupCheck", groupCheck);
											roomInfo.put("password", roomPasswd);
											JSONArray arr = new JSONArray();
											JSONObject po = new JSONObject();
											po.put("ow",uname);
											po.put("id",uname);
											po.put("ready", readyCheck);
											po.put("message","");
											arr.add(po);
											roomInfo.put("playerList", arr);
											
											//rHash = roomInfo.hashCode();
											//roomHash.put(rHash,roomInfo);
											//System.out.println("playerList"+roomInfo);
											createdRoom.add(roomInfo);
											rob.put("roomList",createdRoom);
											

											//System.out.println("룸 리스트"+createdRoom);
											//System.out.println("리스트에 몇번째에 있나요?"+createdRoom.indexOf(roomInfo));
											//System.out.println("방의 해쉬번호 "+ rHash); 
											
											
											// 방정보를 저장하고있는 json 파일을 업데이트한다.  
											try (FileWriter file = new FileWriter(roomFilePath)) // json 파일에 문자를 작성한다. 
											{
												file.write(rob.toString()); // jsonobject에 들어있는 문자를 입력 
												file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
											}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
											
											// addRoom에 추가할 방의 정보를 담아서 접속한 모든 클라이언트에게 보내준다.  
											JSONObject addRoom = new JSONObject();
											addRoom.put("addRoom", roomInfo);
											String addRoomJson = JSONValue.toJSONString(addRoom);
											sendAll(addRoomJson);
											// System.out.println(addRoomJson);
											
											// 방을만든 클라이언트의 상태 업데이트 
											//try (FileWriter file = new FileWriter(playerFilePath)) // json 파일에 문자를 작성한다. 
											//{
												//file.write(job.toString()); // jsonobject에 들어있는 문자를 입력 
												//file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
											//}catch(IOException e){e.printStackTrace();}// 파일에 추가하기
											
											JSONObject udpInfo = new JSONObject();
											JSONArray udparr = new JSONArray();

											udpInfo.put("owner",uname);
											udpInfo.put("playerList",udparr);
											
											udpRoom.add(udpInfo);
											uob.put("udpRoom",udpRoom);
											
											// 방정보를 저장하고있는 json 파일을 업데이트한다.  
											try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/udpRoom.json")) // json 파일에 문자를 작성한다. 
											{
												file.write(uob.toString()); // jsonobject에 들어있는 문자를 입력 
												file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
											}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
							
											
											roomCount=true;
									}
									}
									// 클라이언트가 방을 생성했을 때!!!
									//-----------------------------------------------------------------------------------------------------------
									// 다른사람이 많든 방에 들어갈때 
									if(inRoom && !roomOwner) 
									{
										if(!roomCount&&!refreshOn) {
											list.get(list.indexOf(pwriter)).println(roomJson); //list에서 현재 클라이언트의 인덱스번호로 클라이언트를 구분한다.
											list.get(list.indexOf(pwriter)).flush();		
												roomCount = true;
										}
									
											if(ready) {
												// 메세지전송 
												if (!refreshOn&&!getMessage.equals("")&&!getMessage.equals("readyonoffvity")) {
													String msgJson = JSONValue.toJSONString(p);
													//로비에있는 채팅방에 글을 올린다. 
													sendAll(msgJson);
													//System.out.println("방장 메세지전송");
												}
												else if(!refreshOn&&getMessage.equals("readyonoffvity")) {
													for(int i = 0; i<createdRoom.size(); i++) 
													{
														JSONObject tmp = (JSONObject)createdRoom.get(i); //인덱스 번호로 접근 
														String ow = (String)tmp.get("owner");
														//현재 방에있는 오너를 찾는다. 
														if(ow.equals(ownerName))
														{
																JSONArray pList = (JSONArray) tmp.get("playerList");
																for(int j = 0; j<pList.size(); j++) {
																	JSONObject tmp2 = (JSONObject) pList.get(j);
																	String id = (String) tmp2.get("id");
																	
																	//나의 아이디를 찾는다. 
																	if(id.equals(name)) {
																		
																		tmp2.replace("ready", readyCheck);
																		
																		if(!readyCheck) {
																			JSONObject readyOffId = new JSONObject();
																			readyOffId.put("id", name);
																			readyOffId.put("ready", readyCheck);
																			readyOffId.put("ow", ow);
																			
																			JSONObject readyOffPlayer = new JSONObject();
																			readyOffPlayer.put("readyOffPlayer", readyOffId);
																			String readyOffPlayerJson = JSONValue.toJSONString(readyOffPlayer);
																			sendAll(readyOffPlayerJson);
																		}
																		else{
																			JSONObject readyId = new JSONObject();
																			readyId.put("id", name);
																			readyId.put("ready", readyCheck);
																			readyId.put("ow", ow);
																			
																			JSONObject readyPlayer = new JSONObject();
																			readyPlayer.put("readyPlayer", readyId);
																			String readyPlayerJson = JSONValue.toJSONString(readyPlayer);
																			sendAll(readyPlayerJson);
																		}
																	}
																}
														}
													}
												}
												
												
											}
											else if(!ready) {
												if(playerCount>0 && playerCount<5) {
													//System.out.println("방장이름 : " + ownerName);
													//System.out.println("인원수 : " + playerCount);	
													// 룸정보가 담긴 어레이리스트 확인 
													for(int i = 0; i<createdRoom.size(); i++) 
													{
														JSONObject tmp = (JSONObject)createdRoom.get(i); //인덱스 번호로 접근 
														String ow = (String)tmp.get("owner");
														//boolean pr = ((boolean)tmp.get("private")); 
														//String pw = ((String)tmp.get("password").toString());
														//들어온 방의 방장이름으로 룸 리스트를 검색한다. 
														
														if(ow.equals(ownerName))
														{
															
																//System.out.println("ow" + ow);
																//System.out.println("ownerName" + ownerName);
																JSONArray pList = (JSONArray) tmp.get("playerList");
																// 디스커넥트하는 클라이언트가 저장되어있는 게임방 플레이어리스트에 접근 
																//for(int j = 0; j<pList.size(); j++) {
																	//JSONObject tmp2 = (JSONObject) pList.get(j);
																	//String id = (String) tmp2.get("id");
																	//System.out.println("방에있는 유저"+id);
															//	}
															
																//System.out.println("playerList"+pList);
																//System.out.println("rob" + rob);
																
																if(!refreshOn) {
																	JSONObject addId = new JSONObject();
																	addId.put("id", name);
																	addId.put("ready", readyCheck);
																	addId.put("message", "");
																	addId.put("ow", ow);
																	pList.add(addId);
																	//System.out.println("손님 입장하면 리스트 추가");
																	JSONObject addPlayer = new JSONObject();
																	addPlayer.put("addPlayer", addId);
																	String addPlayerJson = JSONValue.toJSONString(addPlayer);
																	sendAll(addPlayerJson);
																	//System.out.println(addPlayerJson);
																	ready = true;
																}	
														}
													}
													
												}
												if(playerCount>4) {
													System.out.println("인원수 초과");
												}
												
											}
											
											if(refreshOn) 
											{
												//System.out.println("방에서 새로고침버튼 누름!");
												//JSONObject user = (JSONObject) connectPlayer.get(name);
												//System.out.println("새로고침한 클라이언트 : "+user);
												//user.replace("refreshOn", false); 
												//System.out.println("새로고침 다시 false : " +user);
												//System.out.println("connectPlayer : "+connectPlayer);
												//System.out.println("job : " + job);
												list.get(list.indexOf(pwriter)).println(roomJson); //list에서 현재 클라이언트의 인덱스번호로 클라이언트를 구분한다.
												list.get(list.indexOf(pwriter)).flush();	
												
											}

									
										// 방정보를 저장하고있는 json 파일을 업데이트한다.  
										try (FileWriter file = new FileWriter(roomFilePath)) // json 파일에 문자를 작성한다. 
										{
											file.write(rob.toString()); // jsonobject에 들어있는 문자를 입력 
											file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
										}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
										
							
									}
									// 다른사람이 많든 방에 들어갈때 
									//-----------------------------------------------------------------------------------------------------------
								}
								// ----------------------접속 후에 전달받는 메세지 	
								
								
								// ------------------------처음서버접속시
								if(!connect) {
									// 처음 접속할때 현재 게임방을 업데이트 해준다.  
									//개별 전송  방정보는 주고받는게 아니기때문에 개개인한테 따로 보내준다 
									list.get(list.indexOf(pwriter)).println(roomJson); //list에서 현재 클라이언트의 인덱스번호로 클라이언트를 구분한다.
									list.get(list.indexOf(pwriter)).flush();	
									// 접속한 유저의 이름으로 유저의 정보를 넣는다. 
									
									connect = true;
								}
								// ------------------------처음서버접속시	
								
								
								
					  
							}catch(Exception e) {System.out.println(e.getMessage());}  // 1차로 클라이언트 메세지를 파싱한
							
							}
						//클라이언트 메세지를 한줄 파싱할때 while문 			
				}catch(Exception e){
					System.out.println(e.getMessage());
				}finally { // finally는 반드시 실행해야하는 부분 
					list.remove(pwriter);
					//sendAll(name+"is out");
					System.out.println(name + " disconnect"); 
					System.out.println("list : " + list); 
					
					try {
						//------------------------접속한 클라이언트 리스트에서 지우기 
						JSONParser jsonParser = new JSONParser(); // 접속한 유저를 저장하는 json 파일 파싱하기 
			
						//1***********************게임방에있었다면 플레이리스트에서 지우기
						//게임룸 정보가 저장되는 json 파일 파싱해서 플레이어에게 보여준다. 
						Object robj = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/roomList.json"));
						Object uobj = jsonParser.parse(new FileReader("/home/ubuntu/Vity-server/udpRoom.json")); // 
						JSONObject rob = (JSONObject) robj;
						JSONObject uob = (JSONObject) uobj;
						JSONArray createdRoom = (JSONArray) rob.get("roomList");
						JSONArray udpRoom = (JSONArray) uob.get("udpRoom");
						//System.out.println("rob : "+rob);
							//System.out.println("roomList : "+ createdRoom);
							
						// 룸정보가 담긴 어레이리스트 확인 
							for(int i = 0; i<createdRoom.size(); i++) {
								JSONObject tmp = (JSONObject)createdRoom.get(i); //인덱스 번호로 접근 
								
								String ow = (String)tmp.get("owner");
								//String rn = (String)tmp.get("roomName");
								//System.out.println(i+"번째"+"owner : "+ow);
								//System.out.println("현재유저는 : " + name);
								
								// 방장에 디스커넥트하는 클라이언트의 이름이 있으면  
								if(ow.equals(name)) {
									//System.out.println(name+"님이 만든방이 있습니다.");
									JSONArray pList = (JSONArray) tmp.get("playerList");
									//System.out.println("플레이어리스트 : "+pList);	
									
									// 디스커넥트하는 클라이언트가 저장되어있는 게임방 플레이어리스트에 접근 
									for(int j = 0; j<pList.size(); j++) {
										JSONObject tmp2 = (JSONObject) pList.get(j);
										String id = (String) tmp2.get("id");
										//System.out.println("플레이어리스트 방장 : " + id);
										//게임방 플레이어 리스트에서 디스커넥트하는 클라이언트의 이름을 지운다. 
										if(id.equals(name)) {
											
											
											// 게임방 참여자 1명일때 
											if(pList.size()==1) {
												pList.remove(j);
												// 해당방을 지운다. 
												createdRoom.remove(tmp); 
												//System.out.println("게임방리스트 : " + createdRoom);	
												// udproom도 지운다. 
												for(int u = 0; u < udpRoom.size(); u++) {
													JSONObject utmp = (JSONObject)udpRoom.get(u);
													String uow = (String)utmp.get("owner");
													if(uow.equals(name)) {
														udpRoom.remove(utmp);
													}
												}
												
											
											}
											
											// 방에 사람이 있을때
											else if (pList.size()>1)
											{  
												JSONObject tmp3 = (JSONObject) pList.get(j+1);
												//System.out.println("게임방에있는 다른유저 : " + tmp3);
												String id2 = (String) tmp3.get("id");
												//System.out.println("다른유저의 id : " + id2);
												//System.out.println("디스커넥트 유저가 사라진 리스트" + pList);	
												for(int o = 0; o < udpRoom.size(); o++) {
													JSONObject utmp = (JSONObject)udpRoom.get(o);
													String uow = (String)utmp.get("owner");
													if(uow.equals(name)) {
														utmp.replace("owner", id2);
													}
												}
												tmp.replace("owner", id2);
												pList.remove(j);
												//System.out.println("방장이 변경된 게임방 : "+tmp);
											}
											// 방에 사람이 있을때
											
										}//게임방 플레이어 리스트에서 디스커넥트하는 클라이언트의 이름을 지운다. 
									}// 디스커넥트하는 클라이언트가 저장되어있는 게임방 플레이어리스트에 접근 		
								}// 방장에 디스커넥트하는 클라이언트의 이름이 있으면							
							
							
								//방문자가 방을 나갈때 
								if(!ow.equals(name)) {
									JSONArray pList = (JSONArray) tmp.get("playerList");
									// 디스커넥트하는 클라이언트가 저장되어있는 게임방 플레이어리스트에 접근 
									for(int j = 0; j<pList.size(); j++) {
										JSONObject tmp2 = (JSONObject) pList.get(j);
										
										String id = (String) tmp2.get("id");
										if(id.equals(name)) {
												pList.remove(j);
												//System.out.println("ow" + ow);
												//System.out.println("ownerNmae"+ownerName);
												//System.out.println("pList" + pList);
												//System.out.println("방문자가 방에서 나갈때");
												
												JSONObject removeId = new JSONObject();
												removeId.put("id", name);
												removeId.put("message", "");
												removeId.put("ow", ow);
												JSONObject removePlayer = new JSONObject();
												removePlayer.put("removePlayer", removeId);
												String removePlayerJson = JSONValue.toJSONString(removePlayer);
												sendAll(removePlayerJson);
												//System.out.println(addPlayerJson);
											}
										}
								}
							
							}// 룸정보가 담긴 어레이리스트 확인 
							//System.out.println("남아있는 roomList : " + rob);
							//System.out.println("남아있는 playerList : " + job);
							// 방정보를 저장하고있는 json 파일을 업데이트한다.  
							try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/roomList.json")) // json 파일에 문자를 작성한다. 
							{
								file.write(rob.toString()); // jsonobject에 들어있는 문자를 입력 
								file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
							}catch(IOException e){e.printStackTrace();}// 파일에 추가하기
						//1***********************게임방에있었다면 플레이리스트에서 지우기 
							
							// 방정보를 저장하고있는 json 파일을 업데이트한다.  
							try (FileWriter file = new FileWriter("/home/ubuntu/Vity-server/udpRoom.json")) // json 파일에 문자를 작성한다. 
							{
								file.write(uob.toString()); // jsonobject에 들어있는 문자를 입력 
								file.flush();//stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
							}catch(IOException e){e.printStackTrace();}// 파일에 추가하기	
			
					 
							

						socket.close(); // 소켓 객체를 닫는다. 
					
					}catch(Exception ignored) {
						
					}
				}
	}
	
	
	//서버에 접속한 모든 클라이언트에게 같은 메세지를 보낸다. 
	private void sendAll(String str) {
		for(PrintWriter writer : list) {
			writer.println(str);
			// flush() 메소드는 stream에 남아있는 데이터를 강제로 내보내는 역할을 한다. 
			// 호스에 고인물을 빼내는 느낌?  
			writer.flush();
		}
	}

}