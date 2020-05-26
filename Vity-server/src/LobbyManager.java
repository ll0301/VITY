
import java.net.ServerSocket;
import java.net.Socket;

public class LobbyManager extends Thread {
	// 서버 소켓을 빈값으로 생성한다. 
	ServerSocket serverSocket = null; 
	// 포트번호 
	int port = 6321;
	
	public void run() {

		 try{
			 // 6321포트에서 동작하는 서버소켓을 생성한다. 
	         serverSocket = new ServerSocket(port);  
	         System.out.println("Lobby 접속 대기중..."); 
	         while(true) {
	        	 // .accept()  메소드를 실행해서 클라이언트의 접속을 대기한다. 
	        	 Socket socket = serverSocket.accept();
	        	 
	        	 // 멀티접속을 하게 해줄 쓰레드를 소켓과 연결하여 생성한다. 
	        	 Thread thread = new PerClientThread(socket);
	        	 // 쓰레드 실행
	        	 thread.start();
	         }
	  } catch(Exception e){
	         System.out.println(e.getMessage());
	  }
		
	}
	
}
