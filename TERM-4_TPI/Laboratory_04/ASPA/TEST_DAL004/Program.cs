using DAL004;

internal class Program
{
    private static void Main(string[] args)
    {
        JsonRepository.json_filename = "Celebrities.json";

        using (Repository repository = new JsonRepository("."))
        {
            void print(string label)
            {
                Console.WriteLine($"\n--- {label} ---");
                var all = repository.getAllCelebrities();
                foreach (var c in all)
                {
                    Console.WriteLine($"Id: {c.celebrity_id}, Name: {c.firstname} {c.surname}, Path: {c.photo_path}");
                }
            }

            print("Start state");

            int? testdel1 = repository.addCelebrity(new Celebrity { firstname = "TestDell", surname = "TestDell", photo_path = "Photo/TestDell.jpg" });
            int? testdel2 = repository.addCelebrity(new Celebrity { firstname = "TestDel2", surname = "TestDel2", photo_path = "Photo/TestDel2.jpg" });
            int? testupd1 = repository.addCelebrity(new Celebrity { firstname = "TestUpd1", surname = "TestUpd1", photo_path = "Photo/TestUpdl.jpg" });
            int? testupd2 = repository.addCelebrity(new Celebrity { firstname = "TestUpd2", surname = "TestUpd2", photo_path = "Photo/TestUpd2.jpg" });

            repository.saveChange();
            print("After add 4");

            if (testdel1 != null)
            {
                if (repository.delCelebityById((int)testdel1))
                    Console.WriteLine($"Successfully deleted id: {testdel1}");
            }

            if (testdel2 != null)
            {
                if (repository.delCelebityById((int)testdel2))
                    Console.WriteLine($"Successfully deleted id: {testdel2}");
            }

            if (!repository.delCelebityById(1000))
                Console.WriteLine("Delete id 1000: Error (not found)");

            repository.saveChange();
            print("After del 2");

            if (testupd1 != null)
            {
                repository.updCelebityById((int)testupd1, new Celebrity
                {
                    firstname = "Updated1",
                    surname = "Updated1",
                    photo_path = "Photo/Updated1.jpg"
                });
            }

            if (testupd2 != null)
            {
                repository.updCelebityById((int)testupd2, new Celebrity
                {
                    firstname = "Updated2",
                    surname = "Updated2",
                    photo_path = "Photo/Updated2.jpg"
                });
            }

            repository.saveChange();
            print("upd 2");
        }
    }
}