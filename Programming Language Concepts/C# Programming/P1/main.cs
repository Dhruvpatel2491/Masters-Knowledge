using System;

// Define interfaces
// public interface Ia
// {
//     void f();
//     void g();
//     void h1();
// }

// public interface Ib
// {
//     void f();
//     void g();
//     void h2();
// }

// Class A definition from the provided dll
// public class A
// {
//     public virtual void f()
//     {
//         Console.WriteLine("A's f");
//     }

//     public virtual void g()
//     {
//         Console.WriteLine("A's g");
//     }

//     public virtual void h1()
//     {
//         Console.WriteLine("A's h1");
//     }
// }

// // Class B definition from the provided dll
// public class B
// {
//     public virtual void f()
//     {
//         Console.WriteLine("B's f");
//     }

//     public virtual void g()
//     {
//         Console.WriteLine("B's g");
//     }

//     public virtual void h2()
//     {
//         Console.WriteLine("B's h2");
//     }
// }

// Class C definition
public class C : Ia, Ib
{
    private A instanceA = new A(); // Composition: Instance of class A

    public void f()
    {
        instanceA.f(); // Call A's implementation
    }

    public void g()
    {
        new B().g(); // Call B's implementation
    }

    public void h1()
    {
        instanceA.h1(); // Call A's implementation
    }

    public void h2()
    {
        new B().h2(); // Call B's implementation
    }
}

public class Program
{
    public static void Main()
    {
        C n = new C();
        n.f(); n.g(); n.h1(); n.h2();
    }
}
