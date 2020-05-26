

//TCP 채팅  
public class ServerMain{

	//바이트 데이터 
	public static byte[] getbyte = new byte[1024];

	public static void main (String[] args) {

		
		Thread thread1 = new NetworkManager();
   	 	thread1.start();
   	 	
   	 	Thread thread2 = new LobbyManager();
	 	thread2.start();
	 	
	 	Thread thread3 = new TransformReceiveThread();
	 	thread3.start();
	 	
	 	Thread thread4 = new TransformUpdateThread();
	 	thread4.start();
	 	
	 	Thread thread5 = new NPCTransformUpdateThread();
	 	thread5.start();
	 	
	 	Thread thread6 = new NPCTransformReceiveThread();
	 	thread6.start();

	 	//Thread thread7 = new ItemTransformUpdateThread();
	 	//thread7.start();
		
	}
	
}
