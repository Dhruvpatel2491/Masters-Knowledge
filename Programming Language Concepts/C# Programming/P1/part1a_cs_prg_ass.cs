using System;
// using csasn1bases;


public class C : A, Ia, Ib
{
    private B b_instance = new B();

    public override void f()
    {
        base.f();
    }

    public override void h1()
    {
        base.h1();
    }

    public override void g()
    {
        b_instance.g(); 
    }

    public void h2()
    {
        b_instance.h2();
    }
}

public class P1A
{
    public static void Main()
    {
        C n = new C();
        n.f(); n.g(); n.h1(); n.h2();
    }
}