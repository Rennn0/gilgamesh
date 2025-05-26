namespace EducativeIo.BoundedBuffer
{
    public enum User
    {
        None, Male, Female
    }
    public class UnisexRoomTest
    {
        public void run()
        {
            UnisexRoom bathroom = new UnisexRoom();

            Thread female1 = new Thread(new ParameterizedThreadStart(bathroom.FemaleEnter));
            Thread male1 = new Thread(new ParameterizedThreadStart(bathroom.MaleEnter));
            Thread male2 = new Thread(new ParameterizedThreadStart(bathroom.MaleEnter));
            Thread female2 = new Thread(new ParameterizedThreadStart(bathroom.FemaleEnter));
            Thread male3 = new Thread(new ParameterizedThreadStart(bathroom.MaleEnter));
            Thread male4 = new Thread(new ParameterizedThreadStart(bathroom.MaleEnter));
            Thread male5 = new Thread(new ParameterizedThreadStart(bathroom.MaleEnter));
            Thread male6 = new Thread(new ParameterizedThreadStart(bathroom.MaleEnter));

            female1.Start("Lisa");
            male1.Start("John");
            male2.Start("Bob");
            Thread.Sleep(1000);
            female2.Start("Natasha");
            male3.Start("Anil");
            male4.Start("Wentao");
            male5.Start("Nihkil");
            male6.Start("Paul");

            female1.Join();
            female2.Join();
            male1.Join();
            male2.Join();
            male3.Join();
            male4.Join();
            male5.Join();
            male6.Join();

            Console.WriteLine(String.Format("Employees in bathroom at the end {0}", bathroom.getEmpsInBathroom()));
        }
    }
    public class UnisexRoom
    {
        private User m_user;
        private int m_users;
        private readonly object mr_sync;
        public UnisexRoom()
        {
            m_user = User.None;
            m_users = 0;
            mr_sync = new object();
        }

        public int getEmpsInBathroom() => m_users;
        public void UseRoom(object? name)
        {
            Console.WriteLine($"{name} {m_user} using room, {m_users} users in room");
            Thread.Sleep(3000);
            Console.WriteLine($"{name} {m_user} leaving room, {m_users - 1} users left in room");
        }

        public void MaleEnter(object? name)
        {
            Monitor.Enter(mr_sync);
            while (m_user == User.Female || m_users >= 3)
            {
                Monitor.Wait(mr_sync);
            }
            m_user = User.Male;
            m_users++;
            Monitor.Exit(mr_sync);

            UseRoom(name);

            Monitor.Enter(mr_sync);
            m_users--;
            if (m_users == 0)
                m_user = User.None;

            Monitor.PulseAll(mr_sync);
            Monitor.Exit(mr_sync);
        }

        public void FemaleEnter(object? name)
        {
            Monitor.Enter(mr_sync);
            while (m_user == User.Male || m_users >= 3)
            {
                Monitor.Wait(mr_sync);
            }
            m_user = User.Female;
            m_users++;
            Monitor.Exit(mr_sync);

            UseRoom(name);

            Monitor.Enter(mr_sync);
            m_users--;
            if (m_users == 0)
                m_user = User.None;

            Monitor.PulseAll(mr_sync);
            Monitor.Exit(mr_sync);
        }
    }
}