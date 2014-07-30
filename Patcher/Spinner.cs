using System;
using System.ComponentModel;
using System.Threading;

namespace Patcher2
{
  public static class Spinner
  {
    private static BackgroundWorker spinner = initialiseBackgroundWorker();
    private static int spinnerPosition = 25;
    private static int spinWait = 25;
    private static bool isRunning;

    public static bool IsRunning { get { return isRunning; } }

    private static BackgroundWorker initialiseBackgroundWorker()
    {
      BackgroundWorker obj = new BackgroundWorker();
      obj.WorkerSupportsCancellation = true;
      obj.DoWork += delegate //anonymous method for background thread's DoWork event
      {
        spinnerPosition = Console.CursorLeft;
        while (!obj.CancellationPending) //run animation unless a cancellation is pending
        {
          char[] spinChars = new char[] { '|', '/', '-', '\\' };
          foreach (char spinChar in spinChars)
          {
            Console.CursorLeft = spinnerPosition;
            Console.Write(spinChar);
            Thread.Sleep(spinWait);
          }
        }
      };
      return obj;
    }

    public static void Start(int spinWait)
    {
      isRunning = true;
      Spinner.spinWait = spinWait;
      if (!spinner.IsBusy) //start the animation unless already started
        spinner.RunWorkerAsync();
      else throw new InvalidOperationException("Cannot start spinner whilst spinner is already running.");
    }

    public static void Start()
    {
      Start(100);
    }

    public static void Stop()
    {
      //Stop the animation
      spinner.CancelAsync();
      //wait for cancellation to complete
      //while (spinner.IsBusy) Thread.Sleep(100);
      Console.CursorLeft = spinnerPosition;
      isRunning = false;
    }
  }
}