namespace EducativeIo.BoundedBuffer;

public class DiningPhilosophersTest
{
    public void Run()
    {
        DiningPhilosophers problem = new DiningPhilosophers(5);

        Thread[] philosophers = new Thread[5];

        for (int i = 0; i < 5; i++)
            philosophers[i] = new Thread(new ParameterizedThreadStart(problem.Lifecycle));

        for (int i = 0; i < 5; i++)
            philosophers[i].Start(i);

        Thread.Sleep(16000);

        problem.SetExit();

        for (int i = 0; i < 5; i++)
            philosophers[i].Join();
    }
}

public class DiningPhilosophers
{
    private readonly Semaphore[] mr_forks;
    private readonly Semaphore mr_dinners;
    private volatile bool m_exit;

    public DiningPhilosophers(int philosophers)
    {
        mr_forks = new Semaphore[philosophers];
        for (int i = 0; i < philosophers; i++)
        {
            mr_forks[i] = new Semaphore(initialCount: 1, maximumCount: 1);
        }

        mr_dinners = new Semaphore(philosophers - 1, philosophers - 1);
        m_exit = false;
    }

    public void Lifecycle(object? id)
    {
        while (!m_exit)
        {
            Contemplate();
            Eat((int)id!);
        }
    }

    public void Eat(int id)
    {
        mr_dinners.WaitOne();

        mr_forks[id].WaitOne();
        mr_forks[(id + mr_forks.Length - 1) % mr_forks.Length].WaitOne();

        Console.WriteLine($"Philosopher {id} is eating.");

        mr_forks[id].Release();
        mr_forks[(id + mr_forks.Length - 1) % mr_forks.Length].Release();

        mr_dinners.Release();
    }

    public void Contemplate()
    {
        Thread.Sleep(new Random().Next(1000, 4000));
    }

    public void SetExit() => m_exit = true;
}
