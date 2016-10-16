using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DataSt;



class Programm
{

  static void Main(string[] args)
  {

    IPAddress _ipa = IPAddress.Parse("10.0.1.46");
    int port = int.Parse("8181");
    IPEndPoint ipe = new IPEndPoint(_ipa, port);

    Socket _sListerner = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    try
    {
      _sListerner.Bind(ipe);
      _sListerner.Listen(3);

      while (true)
      {
        float _speed = 1;
        Console.WriteLine("wait connection from 10.0.1.46:8181");
        Socket handler = _sListerner.Accept();
        string inputmassage = null;
        byte[] bytes = new byte[1024];
        int bytesRec = handler.Receive(bytes);
        inputmassage += Encoding.UTF8.GetString(bytes, 0, bytesRec);
        Console.WriteLine("Полученный текст " + inputmassage + "\n\n");
        //handler.Send(Encoding.UTF8.GetBytes("<END>"));
        if (inputmassage.IndexOf("<VideoSystem>") > -1)
        {

          try
          {
            while (true)
            {
              Data _d = new Data() { coordinate = 1, currentSpeed = _speed += 2, direction = 1, modelTime = 0, nameRailRoad = "PAchev АРЕТМ", typeTrain = 1 };
              handler.Send(Data.Serialize(_d));
              //handler.Send(Encoding.UTF8.GetBytes("1"));
              Console.WriteLine(_speed);
              Thread.Sleep(40);
            }
          }
          catch (SocketException ex)
          {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            Console.WriteLine("WorkExeption");
            break;
          }


          handler.Shutdown(SocketShutdown.Both);
          handler.Close();
          Console.WriteLine("Сервер завершил соеденение с клиентом");
          Console.WriteLine(handler.RemoteEndPoint);
          break;
        }
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
      }
    }
    catch (Exception ex)
    {
      //Console.WriteLine(ex.ToString());
      Console.WriteLine("Working LastEx");
      Main(new string[1]);
    }
    finally
    {
      Console.WriteLine("Finaly");
      //Main(new string[1]);
      //Console.ReadLine();
    }

  }
  //finally
  //{
  //  //Main(new string[1]);
  //  Console.ReadLine();
  //}
  static bool k = true;



}

