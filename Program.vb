Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Text

Module Program
    Dim Address As String
    Dim ReadOnly Port = 443
    Sub Main()
        Dim Server = New TCPListener(IPAddress.Parse("0.0.0.0"), Port)
        Server.Start()
        Console.WriteLine(String.Format("Listening on port {0}. Waiting for connection...", Port))
        While (True)
            Address = ReceiveData(Server)
            Console.WriteLine(String.Format("Connection from {0} on {1}.", Address, Port))
            Console.WriteLine(String.Format("Running network mapper on {0} at {1}", Address, DateTime.Now))
            Dim Map = ScanNetwork(Address)
            Console.WriteLine(Map)
            SaveOutput(Map)
        End While
    End Sub
    Private Sub SaveOutput (Output As String)
        Using SW As StreamWriter = File.CreateText(String.Format("{0:mm-dd-yy H:mm:ss}.txt", Date.Now))
            SW.Write(Output)
            SW.Close()
        End Using
    End Sub
    Private Function ScanNetwork(Address As String) As String
        Dim Output = New Process()
        Output.StartInfo.RedirectStandardOutput = True
        Output.StartInfo.FileName = ("/usr/bin/nmap")
        Output.StartInfo.Arguments = Address
        Output.Start()
        Dim Map = Output.StandardOutput.ReadToEnd()
        Output.WaitForExit()
        Return Map
    End Function
    Private Function ReceiveData(Server As TcpListener) As String
        Dim Client = Server.AcceptTcpClient()
        While (Client.Client.Connected)
            Dim NS As NetworkStream = Client.GetStream()
            If NS.CanRead Then
                Dim IP(Client.ReceiveBufferSize) As Byte
                NS.Read(IP, 0, CInt(Client.ReceiveBufferSize))
                Address = Encoding.ASCII.GetString(IP)
            End If
            Client.Close()
        End While
        Return Address
    End Function
End Module
