public class SocketImplImpl extends java.net.SocketImpl {  java.net.Socket socket;  java.net.ServerSocket serverSocket; protected java.io.FileDescriptor fd; protected java.net.InetAddress address; protected int port; protected int localport; public SocketImplImpl() {}
protected java.net.InetAddress getInetAddress() { return null;  } 
protected int getLocalPort() { return 0;  } 
protected  int available() { return 0;  } 
 java.net.ServerSocket getServerSocket() { return null;  } 
 java.lang.Object getOption(java.net.SocketOption arg0) { return null;  } 
public  java.lang.Object getOption(int arg0) { return null;  } 
protected  void listen(int arg0) { return;  } 
protected  void bind(java.net.InetAddress arg0,int arg1) { return;  } 
protected boolean supportsUrgentData() { return true;  } 
 void setServerSocket(java.net.ServerSocket arg0) { return;  } 
protected java.io.FileDescriptor getFileDescriptor() { return null;  } 
protected  void create(boolean arg0) { return;  } 
protected  void close() { return;  } 
protected  void connect(java.net.SocketAddress arg0,int arg1) { return;  } 
protected  void connect(java.lang.String arg0,int arg1) { return;  } 
protected  void connect(java.net.InetAddress arg0,int arg1) { return;  } 
public  void setOption(int arg0,java.lang.Object arg1) { return;  } 
 void setOption(java.net.SocketOption arg0,java.lang.Object arg1) { return;  } 
 void setSocket(java.net.Socket arg0) { return;  } 
protected  java.io.InputStream getInputStream() { return null;  } 
protected void shutdownOutput() { return;  } 
protected  void accept(java.net.SocketImpl arg0) { return;  } 
protected void shutdownInput() { return;  } 
protected void setPerformancePreferences(int arg0,int arg1,int arg2) { return;  } 
protected  void sendUrgentData(int arg0) { return;  } 
protected int getPort() { return 0;  } 
 java.net.Socket getSocket() { return null;  } 
 void reset() { return;  } 
protected  java.io.OutputStream getOutputStream() { return null;  } 
public java.lang.String toString() { return null;  } 
}