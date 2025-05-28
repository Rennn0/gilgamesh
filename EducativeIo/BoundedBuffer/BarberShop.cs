namespace EducativeIo.BoundedBuffer;

public class BarberShopTest
{
    public void Run()
    {
        int id = 1;
        BarberShop barberShop = new BarberShop();

        Thread barberThread = new Thread(new ThreadStart(barberShop.Barber));

        barberThread.Start();

        Thread[] customers = new Thread[10];
        for (int i = 0; i < 10; i++)
        {
            customers[i] = new Thread(new ParameterizedThreadStart(barberShop.CustomerEnters));
        }

        for (int i = 0; i < 10; i++)
        {
            customers[i].Start(id);
            id++;
        }

        Thread.Sleep(500);

        Thread[] lateCustomers = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            lateCustomers[i] = new Thread(new ParameterizedThreadStart(barberShop.CustomerEnters));
        }

        for (int i = 0; i < 5; i++)
        {
            lateCustomers[i].Start(id);
            id++;
        }

        for (int i = 0; i < 10; i++)
        {
            customers[i].Join();
        }

        for (int i = 0; i < 5; i++)
        {
            lateCustomers[i].Join();
        }
    }
}

public class BarberShop
{
    private readonly int mr_mChairs;
    private int m_customers;
    private int m_haircutsGiven;
    private volatile bool m_shutdown;
    private readonly object mr_lock;
    private readonly Semaphore mr_customerToEnter;
    private readonly Semaphore mr_customerToLeave;
    private readonly Semaphore mr_barberToCut;
    private readonly Semaphore mr_barberToGetReady;

    public BarberShop(int chairs = 3)
    {
        mr_mChairs = chairs;
        m_customers = 0;
        m_haircutsGiven = 0;
        m_shutdown = false;

        mr_lock = new object();
        mr_customerToEnter = new Semaphore(0, chairs);
        mr_customerToLeave = new Semaphore(0, 1);
        mr_barberToCut = new Semaphore(0, 1);
        mr_barberToGetReady = new Semaphore(0, 1);
    }

    public void ShutDown() => m_shutdown = true;

    public void CustomerEnters(object? id)
    {
        Monitor.Enter(mr_lock);
        if (mr_mChairs == m_customers)
        {
            Console.WriteLine($"Customer {id} leaves without a haircut, no chairs available.");
            Monitor.Exit(mr_lock);
            return;
        }

        m_customers++;
        Monitor.Exit(mr_lock);

        mr_customerToEnter.Release();
        mr_barberToGetReady.WaitOne();

        Monitor.Enter(mr_lock);
        m_customers--;
        Console.WriteLine($"Customer {id} gets a haircut. Customers waiting: {m_customers}");
        Monitor.Exit(mr_lock);

        mr_barberToCut.WaitOne();
        mr_customerToLeave.Release();
    }

    public void Barber()
    {
        while (!m_shutdown)
        {
            mr_customerToEnter.WaitOne();
            mr_barberToGetReady.Release();

            m_haircutsGiven++;
            Console.WriteLine($"Barber is cutting hair. Total haircuts given: {m_haircutsGiven}");
            Thread.Sleep(50);

            mr_barberToCut.Release();
            mr_customerToLeave.WaitOne();
        }
    }
}
