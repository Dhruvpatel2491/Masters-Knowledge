using System;


public class baby_A : A2 
{
    protected override void f()  { base.f();  }
    protected override void g()  { base.g();  }
    protected override void h1() { base.h1(); }
}
public class baby_B : B2 
{
    public void baby_f() { base.f(); }
    public void baby_g() { base.g(); }
    public void baby_h2() { base.h2(); }
}


public class C2: baby_A
{
    private baby_B baby_b_instance = new baby_B();

    public new void f()
    {
        base.f();
    }

    public new void h1()
    {
        base.h1();
    }

    public new void g()
    {
        baby_b_instance.baby_g(); 
    }

    public void h2()
    {
        baby_b_instance.baby_h2();
    }
}

public class P1B
{
public static void Main() // main for testing part II
 { 
    C2 n = new C2();   //don't put any print statements in your C2 constructor
    n.f(); 
    n.g(); 
    n.h1(); 
    n.h2();

    //exact output will show proper inheritance
 }
}