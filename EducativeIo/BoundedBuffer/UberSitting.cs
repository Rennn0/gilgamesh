namespace EducativeIo.BoundedBuffer
{
    public class UberSeatingProblemTest
    {
        private Random random = new Random();

        public void run()
        {
            controlledSimulation();
            // randomSimulation();
        }

        public void randomSimulation()
        {
            UberSitting problem = new UberSitting();
            int dems = 0;
            int repubs = 0;

            Thread[] riders = new Thread[16];
            for (int i = 0; i < 16; i++)
            {
                int toss = random.Next(0, 2);

                if (toss == 1)
                {
                    riders[i] = new Thread(new ThreadStart(problem.seatDemocrat));
                    dems++;
                }
                else
                {
                    riders[i] = new Thread(new ThreadStart(problem.seatRepublican));
                    repubs++;
                }
            }

            Console.WriteLine(String.Format("Total {0} dems and {1} repubs", dems, repubs));

            for (int i = 0; i < 16; i++)
            {
                riders[i].Start();
            }

            for (int i = 0; i < 16; i++)
            {
                riders[i].Join();
            }
        }


        public void controlledSimulation()
        {
            UberSitting problem = new UberSitting();
            int dems = 10;
            int repubs = 10;

            int total = dems + repubs;
            Console.WriteLine(String.Format("Total {0} dems and {1} repubs\n", dems, repubs));

            Thread[] riders = new Thread[total];

            while (total != 0)
            {
                int toss = random.Next(0, 2);

                if (toss == 1 && dems != 0)
                {
                    riders[20 - total] = new Thread(new ThreadStart(problem.seatDemocrat));
                    dems--;
                    total--;
                }
                else if (toss == 0 && repubs != 0)
                {
                    riders[20 - total] = new Thread(new ThreadStart(problem.seatRepublican));
                    repubs--;
                    total--;
                }
            }

            for (int i = 0; i < riders.Length; i++)
            {
                riders[i].Start();
            }

            for (int i = 0; i < riders.Length; i++)
            {
                riders[i].Join();
            }
        }
    }

    public class UberSitting
    {
        private int m_democrats;
        private int m_republicans;
        private int m_rides;

        private readonly object mr_padlock;
        private readonly Barrier mr_barrier;
        private readonly Semaphore mr_respublicanSemaphore;
        private readonly Semaphore mr_democratSemaphore;
        public UberSitting()
        {
            m_democrats = 0;
            m_republicans = 0;
            m_rides = 0;

            mr_padlock = new object();
            mr_barrier = new Barrier(4);
            mr_respublicanSemaphore = new Semaphore(0, 99);
            mr_democratSemaphore = new Semaphore(0, 99);
        }
        public void drive()
        {
            m_rides++;
            Console.WriteLine(String.Format("Uber ride # {0} filled and on its way", m_rides));
        }

        public void seated(String party)
        {
            Console.WriteLine(String.Format("\n{0} {1} seated", party, Thread.CurrentThread.ManagedThreadId));
        }

        public void seatDemocrat()
        {
            bool rideLeader = false;

            Monitor.Enter(mr_padlock);
            m_democrats++;

            if (m_democrats == 4)
            {
                rideLeader = true;
                mr_democratSemaphore.Release(3);
                m_democrats -= 4;
            }
            else if (m_democrats == 2 && m_republicans >= 2)
            {
                rideLeader = true;
                mr_democratSemaphore.Release(1);
                mr_respublicanSemaphore.Release(2);
                m_republicans -= 2;
                m_democrats -= 2;
            }
            else
            {
                Monitor.Exit(mr_padlock);
                mr_democratSemaphore.WaitOne();
            }

            seated("Democrat");
            mr_barrier.Arrive();

            if (rideLeader)
            {
                drive();
                Monitor.Exit(mr_padlock);
            }
        }

        public void seatRepublican()
        {
            bool rideLeader = false;

            Monitor.Enter(mr_padlock);
            m_republicans++;

            if (m_republicans == 4)
            {
                rideLeader = true;
                mr_respublicanSemaphore.Release(3);
                m_republicans -= 4;
            }
            else if (m_democrats >= 2 && m_republicans == 2)
            {
                rideLeader = true;
                mr_democratSemaphore.Release(2);
                mr_respublicanSemaphore.Release(1);
                m_republicans -= 2;
                m_democrats -= 2;
            }
            else
            {
                Monitor.Exit(mr_padlock);
                mr_respublicanSemaphore.WaitOne();
            }

            seated("Republican");
            mr_barrier.Arrive();

            if (rideLeader)
            {
                drive();
                Monitor.Exit(mr_padlock);
            }
        }
    }
}