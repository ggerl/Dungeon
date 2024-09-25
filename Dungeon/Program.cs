using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Transactions;

namespace Dungeon
{
    internal class Program
    {


        static void Main(string[] args)
        {
            GameManager gamemanager = new GameManager();

            gamemanager.StartScene();
        }
    }




    public class GameManager
    {
        List<IItem> inventory = new List<IItem>();
        List<IItem> shop = new List<IItem>();
        List<IItem> reward = new List<IItem>(); 

        public static Warrior warrior = new() { Health = 100, Attack = 10 };
        
        public static Stage stage = new Stage();
        public static int stageNum = 0;

        
        
        

        public void StartScene()
        {

            Monster monster;

            

            Console.WriteLine("반갑습니다 처음뵙는군요 당신의 이름은 무엇입니까 ?");
            Console.Write("이곳에 이름을 입력해 주십시오 : ");
            warrior.Name = Console.ReadLine();
            Console.Clear();

            SetItemIntheShop();

            Console.WriteLine($"{warrior.Name}님 마을에 오신걸 환영합니다!");
            Thread.Sleep(1000);
            warrior.gold = 500; // 플레이어 골드 초기값 지정
            do
            {

                if (stageNum <= 0)
                {
                    monster = new Goblin();

                }
                else
                {
                    monster = new Dragon();
                }

                
             


                char Input;
                Console.Clear();
                Console.WriteLine("이곳에서 당신이 할 행동을 지정할 수 있습니다 ");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. 상태를 확인한다");
                Console.WriteLine("2. 상점을 방문한다");
                Console.WriteLine("3. 던전에 입장한다");
                Console.WriteLine("4. 인벤토리를 확인한다");

                Input = Console.ReadKey().KeyChar;
                Console.Clear();

                warrior.Attack = 10;
                if(warrior.CurrentActiveWeapon !=null)  // 플레이어의 무기가 null 이아니면 플레이어의 공격력에 무기의 공격력 합산
                warrior.Attack += warrior.CurrentActiveWeapon.Attack;
                

                switch (Input)
                {
                    case '1':
                        GetUserInfo();
                        break;

                    case '2':
                        OpenShop();
                        break;

                    case '3':
                        stage.StartStage(warrior, monster , reward , inventory, ref stageNum);
                        break;

                    case '4':
                        OpenInventory(inventory);
                        break;


                        
                }





                if (stageNum >= 2)
                {
                    break;
                }

            }
            while (true);

            Console.WriteLine("축하합니다 모든 스테이지를 클리어 하셨습니다 !");
            Console.ReadLine();




        }

        public void OpenInventory(List<IItem> item) // 인벤토리를 연다
        {
            int itemNumber = 1;



           
                foreach (var tem in item) // 매개변수로 받아온 inventory 리스트를 순회하며 출력 이때 현재 착용중인 장비에는 [E]를 같이출력
                {
                    if (warrior.CurrentActiveWeapon != null && tem is Weapon weapon && weapon.Name == warrior.CurrentActiveWeapon.Name )
                    {
                        Console.WriteLine($"{itemNumber}. [E]{weapon.Name} : 공격력 {weapon.Attack} ");


                    }

                    else if (tem is Weapon weapon2)
                    {
                        Console.WriteLine($"{itemNumber}. {weapon2.Name} : 공격력 {weapon2.Attack} ");

                    }


                    else if (tem is Armor armor)
                    {

                        Console.WriteLine($"{itemNumber}. {armor.Name} : 추가 체력 {armor.Health} ");

                    }
                    itemNumber++;
                }

            Console.WriteLine("사용할 장비를 선택해주세요");


            int num;
            bool isInt = int.TryParse(Console.ReadKey().KeyChar.ToString(), out num) ;
            num -= 1;
            Console.Clear();
            if (isInt && num >= 0 && num < item.Count) // 올바른 입력값 구분
            {
                if (item[num] is Weapon selectWeapon && warrior.CurrentActiveWeapon == null) 
                {

                    warrior.CurrentActiveWeapon = selectWeapon;

                }

                else if (item[num] is Weapon selectedWeapon && warrior.CurrentActiveWeapon != null)
                {

                    if (warrior.CurrentActiveWeapon.Name != selectedWeapon.Name)
                    {
                        warrior.CurrentActiveWeapon = selectedWeapon;

                        Console.WriteLine($"{selectedWeapon.Name}이 선택되었습니다.");
                        Console.WriteLine("계속 진행 하시려면 엔터키를 눌러주세요");
                        Console.ReadLine();
                    }

                    else
                    {
                        Console.WriteLine("이미 선택된 무기입니다.");
                        Console.WriteLine("계속 진행 하시려면 엔터키를 눌러주세요");
                        Console.ReadLine();

                    }
                }
            }
            else { Console.WriteLine("잘못된 입력입니다 ! 돌아가시려면 엔터키를 눌러주세요");
                Console.ReadLine();
            }




        }


        
            






        

        public void GetUserInfo() // 유저정보 호출
        {



            Console.WriteLine("유저의 정보입니다");
            Console.WriteLine();
            Console.WriteLine("이름 : " + warrior.Name);
            Console.WriteLine("체력 : " + warrior.Health);

            if(warrior.CurrentActiveWeapon != null) { 
            Console.WriteLine($"공격력 : {warrior.Attack} (+ {warrior.CurrentActiveWeapon.Attack}) "  );
            }
            else
            {
                Console.WriteLine($"공격력 : {warrior.Attack} ");

            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("0. 나가기");



            while (Console.ReadKey(true).KeyChar != '0')
            {

            }
        }



        public void SetItemIntheShop() // 아이템 초기설정 reward 리스트와 shop 리스트에 각각 알맞은 아이템을 할당해준다
        {
            Weapon axe = new Weapon() { Name = "도끼", Attack = 5, price = 100 };
            Weapon spear = new Weapon() { Name = "창", Attack = 10, price = 200 };
            Weapon sword = new Weapon() { Name = "검", Attack = 15, price = 300 };
            Weapon longsword = new Weapon() { Name = "롱소드", Attack = 25 };
            Weapon pike = new Weapon() { Name = "파이크", Attack = 30 };
            shop.Add(axe);
            shop.Add(spear);
            shop.Add(sword);

            reward.Add(longsword);
            reward.Add(pike);


        }

        public void OpenShop() // 상점매서드
        {

            Console.WriteLine("반갑습니다 원하시는 무기를 골라주세요");
            Console.WriteLine();
            Console.WriteLine();

            int itemNumber = 1;


            foreach (var item in shop)  // 상점 리스트를 돌아가며 상점보유 아이템을 출력
            {
                if (item is Weapon weapon)
                {
                    Console.WriteLine($"{itemNumber}. {weapon.Name} : 공격력 {weapon.Attack} , 가격 {weapon.price} ");


                }

                else if (item is Armor armor)
                {

                    Console.WriteLine($"{itemNumber}. {armor.Name} : 추가 체력 {armor.Health} , 가격 {armor.price}");

                }

                itemNumber++;

            }

            char input;
            bool isInt;
            int selectedItem;
            do
            {
                Console.WriteLine("원하시는 장비의 숫자를 입력해 주세요");
                input = Console.ReadKey().KeyChar;
                selectedItem = 0;

                isInt = int.TryParse(input.ToString(), out selectedItem);
                selectedItem -= 1;
            }
            while (!isInt || 0 > selectedItem || selectedItem > shop.Count);



            if (selectedItem >= 0 && selectedItem < shop.Count)
            {
                IItem item = shop[selectedItem];
                BuyItem(item);


            }




            Console.WriteLine("0. 나가기");

            while (Console.ReadKey(true).KeyChar != '0')
            {

            }


        }


        public void BuyItem(IItem item) // 아이템을 구매하는 매서드
        {

            Console.Clear();
            if(warrior.gold >= item.price)
            {
                warrior.gold -= item.price;
                inventory.Add(item);
                Console.WriteLine($"{item.Name} 을 구매하였습니다 , 남은 소지골드는 {warrior.gold} 입니다.");


            }

            else
            {

                Console.WriteLine("소지금이 부족합니다 !");

            }
            Console.WriteLine("0. 확인");

            while (Console.ReadKey().KeyChar != '0') { }
        }
        




    }




            

    public interface ICharacter
        {
            public string Name { get; }
            public int Health { get; set; }

            public int Attack { get; set; }

            public bool IsDead { get; }

        public void TakeDmg() { }
       




        }

        public class Warrior : ICharacter
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Attack { get; set; }

        public bool IsDead => Health <= 0;

        public int gold { get; set; }
        
        public Weapon CurrentActiveWeapon { get; set; }

        public void TakeDmg(int dmg)
        {
            Health -= dmg;
            if (IsDead)
            {

                Console.WriteLine($"{Name} 이 죽었습니다");

            }
            else Console.WriteLine($"{Name} 이 {dmg} 만큼의 데미지를 입었습니다 남은 체력: {Health}");


        }

        }

        public class Monster : ICharacter
        {

            public string Name { get; set; }
            public int Health { get; set; }
            public int Attack { get; set; }
        public bool IsDead => Health <= 0;

        public void TakeDmg(int dmg) {

            Health -= dmg;
            if (IsDead)
            {

                Console.WriteLine($"{Name} 이 죽었습니다");

            }
            else Console.WriteLine($"{Name} 이 {dmg} 만큼의 데미지를 입었습니다 남은 체력: {Health}");

        }   


    }

        public class Goblin : Monster
        {
            public Goblin() { Name = "고블린"; Health = 100; ; Attack = 5; }



        }

        public class Dragon : Monster
        {
            public Dragon() { Name = "드래곤"; Health = 200; Attack = 10; }

        }

        public class Stage
        {

        public void StartStage(Warrior warrior, Monster monster, List<IItem> reward , List<IItem> inven, ref int stageNum)
        {

            Console.WriteLine("던전의 입구입니다 계속 진행하시겠습니까?");
            Console.WriteLine("1. 계속진행한다");
            Console.WriteLine("2. 돌아간다");

            if (Console.ReadKey().KeyChar == '1')
            {
                Console.WriteLine($"던전속에 들어오자말자 적과 조우했습니다 ! 적의정보 : {monster.Name} 공격력 {monster.Attack} , 체력 {monster.Health} ");
                Console.WriteLine("계속 진행하시려면 아무키나 눌러주세요");
                
                Console.ReadLine();
                Console.WriteLine("도망치기엔 이미 늦은거같습니다 적과 교전을 시작합니다.");
                do
                {
                    Console.WriteLine("당신이 공격합니다");
                    monster.TakeDmg(warrior.Attack);

                    Thread.Sleep(1500);
                    if (monster.IsDead)
                    {
                        break;
                    }

                    Console.WriteLine($"{monster.Name}의 차례입니다.");
                    warrior.TakeDmg(monster.Attack);

                    Thread.Sleep(1500);

                    Console.Clear();

                }
                while (!warrior.IsDead && !monster.IsDead);


            }

            else
            {
                Console.WriteLine("마을로 돌아갑니다.");
                Thread.Sleep(2000);


            }


            int itemindex = 0;

            if (monster.IsDead)
            {
                Console.WriteLine("당신의 승리입니다 !");

                if(reward != null)
                {
                    Console.WriteLine("아래의 보상중 하나를 선택해 주세요");
                    Console.WriteLine();
                    Console.WriteLine();


                    foreach(var item in reward)
                    {
                        if(item is Weapon weapon)
                        Console.WriteLine($"{itemindex}. {weapon.Name} : 공격력 {weapon.Attack}");
                        itemindex++;

                    }
                    int num;
                    bool isInt = int.TryParse(Console.ReadKey().KeyChar.ToString(), out num);

                    if (isInt && num >= 0 && num < reward.Count) 
                    {
                        inven.Add(reward[num]);
                        Console.WriteLine($"'{reward[num].Name}' 보상이 지급되었습니다. 계속하시려면 아무키나 눌러주세요");
                        Console.ReadLine();
                        stageNum++;

                        
                    }
                }
            }


            else
            {
                Console.WriteLine("패배했습니다.. 계속하시려면 아무키나 눌러주세요");
                Console.ReadLine();
            }

            warrior.Health = 100;


        
          
        }

    





        }

        public interface IItem
        {
            public string Name { get; set; }
            public int price { get; set; }



        }

        public class Weapon : IItem
        {
            public string Name { get; set; }
            public int price { get; set; }
            public int Attack { get; set; }

        }

        public class Armor : IItem
        {
            public string Name { get; set; }
            public int price { get; set; }
            public int Health { set; get; }

        }

   




    }
