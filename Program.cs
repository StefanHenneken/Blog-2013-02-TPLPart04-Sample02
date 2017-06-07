using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample02
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }
        public void Run()
        {
            Console.WriteLine("Start Run");

            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions po = new ParallelOptions() { CancellationToken = cts.Token };

            Task t = Task.Run(() => Work(po), cts.Token);  // new in .NET 4.5
            // Task t = Task.Factory.StartNew(() => Work(po), cts.Token); // also possible
            Console.WriteLine("Cancel in 3sec");
            cts.CancelAfter(3000);  // new in .NET 4.5

            Console.WriteLine("End Run");
            Console.ReadLine();
        }

        private void Work(ParallelOptions po)
        {
            Console.WriteLine("Start Work");
            Parallel.Invoke(po,
            () =>
            {
                Console.WriteLine("Start Task 1");
                for (int i = 0; i < 10; i++)
                {
                    if (po.CancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancel Task 1");
                        break;
                    }
                    Thread.Sleep(200);
                }
                Console.WriteLine("End Task 1");
            },
            () =>
            {
                Console.WriteLine("Start Task 2");
                for (int i = 0; i < 20; i++)
                {
                    if (po.CancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancel Task 2");
                        break;
                    }
                    Thread.Sleep(200);
                }
                Console.WriteLine("End Task 2");
            },
            () =>
            {
                Console.WriteLine("Start Task 3");
                for (int i = 0; i < 30; i++)
                {
                    if (po.CancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancel Task 3");
                        break;
                    }
                    Thread.Sleep(200);
                }
                Console.WriteLine("End Task 3");
            });
            Console.WriteLine("End Work");
        }
    }
}
