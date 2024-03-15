using System;

public class arithmetics{
  public static bool equals(number a, number b)
  {

          if (a is real)     a=a.upcast(); // A => Complex
      else if (a is integer)  a=a.upcast(); // A => Rational

          if (b is real)     b=b.upcast(); // B =>Complex
      else if (b is integer)  b=b.upcast(); // B=> Rational


      if (a is null || b is null)    return false;


      if (a is rational raA) {

            if (b is rational raB)  return raA.n==raB.n && raA.d==raB.d;

        else if (b is complex cB)    return (new complex(raA.n/raA.d,0).r == cB.r) && (cB.i == 0);
      }

      else if (a is complex cA) {

            if (b is rational raB)  return (new complex(raB.n/raB.d,0).r == cA.r) && (cA.i == 0);

        else if (b is complex cB)    return (cA.r == cB.r) && (cA.i == cB.i);
      }
    
    return false;
          
  }
  
public static number add(number a, number b){

  if (a is null || b is null)    return new integer(0);
    a = a.upcast();
    b = b.upcast();
    return a.match2_commute(
        b,
        II: (ia, ib) => new integer(ia.val + ib.val),
        IF: (ia, fb) => new rational(ia.val * fb.d + fb.n, fb.d),
        IR: (ia, rb) => new real(ia.val + rb.val).upcast(),
        IC: (ia, cb) => new complex(ia.val + cb.r, cb.i),
        FF: (fa, fb) => new rational(fa.n * fb.d + fb.n * fa.d, fa.d * fb.d),
        FR: (fa, rb) => new real(fa.n * 1 + rb.val * fa.d).upcast(),
        FC: (fa, cb) => new complex(fa.n + cb.r / fa.d, cb.i),
        RR: (ra, rb) => new real(ra.val+rb.val),
        RC: (ra, cb) => new complex(ra.val + cb.r, cb.i),
        CC: (ca, cb) => new complex(ca.r + cb.r, ca.i + cb.i)
        );

}

public static number multiply(number a, number b){

  if (a is null || b is null)    return new integer(0);
    a = a.upcast();
    b = b.upcast();

    return a.match2_commute(
        b,
        II: (ia, ib) => new integer(ia.val * ib.val),
        IF: (ia, fb) => new rational(ia.val * fb.n, fb.d),
        IR: (ia, rb) => new real(ia.val * rb.val).upcast(),
        IC: (ia, cb) => new complex(ia.val * cb.r, ia.val * cb.i),
        FF: (fa, fb) => new rational(fa.n * fb.n, fa.d * fb.d),
        FR: (fa, rb) => new real((fa.n * rb.val)/ fa.d),
        FC: (fa, cb) => new complex((fa.n/fa.d) * cb.r , (fa.n/fa.d)*cb.i),
        RR: (ra, rb) => new real(ra.val*rb.val),
        RC: (ra, cb) => new complex(ra.val * cb.r,ra.val * cb.i),
        CC: (ca, cb) => new complex((ca.r*cb.r)-(ca.i*cb.i), (ca.i*cb.r)+(ca.r*cb.i))
    );
}

  public static number sum_arr(number[] A)   //(A has length A.Length)
  {
    if(A.Length==0) return new integer(0);

    number sum = new integer(0);
    for (int i=0;i<A.Length;i++)
        sum = arithmetics.add(sum,A[i]);
  
    return (number)sum;
  }



}

public class testing{

  public static void equal_testing(){
    //Equal Function testing 
    int n=5;
    number i   = new integer(n);
    number re     = new real(n/1.0);
    number ra = new rational(n,1);
    number c   = new complex(n,1); 
    Console.WriteLine("----------------");    
    Console.WriteLine(arithmetics.equals(i,i));
    Console.WriteLine(arithmetics.equals(i,re));
    Console.WriteLine(arithmetics.equals(i,ra));
    Console.WriteLine(arithmetics.equals(i,c));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.equals(ra,i));
    Console.WriteLine(arithmetics.equals(ra,re));
    Console.WriteLine(arithmetics.equals(ra,ra));
    Console.WriteLine(arithmetics.equals(ra,c));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.equals(re,i));
    Console.WriteLine(arithmetics.equals(re,re));
    Console.WriteLine(arithmetics.equals(re,ra));
    Console.WriteLine(arithmetics.equals(re,c));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.equals(c,i));
    Console.WriteLine(arithmetics.equals(c,re));
    Console.WriteLine(arithmetics.equals(c,ra));
    Console.WriteLine(arithmetics.equals(c,c));
    Console.WriteLine("----------------");
  }

  public static void add_testing(){
    int a=5,b=20;
    number ia   = new integer(a);
    number rea     = new real(a/1.0);
    number raa = new rational(a,1);
    number ca   = new complex(a,5); 

    number ib   = new integer(b);
    number reb     = new real(b/1.0);
    number rab = new rational(b,1);
    number cb   = new complex(b,10); 
    Console.WriteLine("ia="+(ia.ToString()));
    Console.WriteLine("ib="+(ib.ToString()));
    Console.WriteLine("rea="+(rea.ToString()));
    Console.WriteLine("reb="+(reb.ToString()));
    Console.WriteLine("raa="+(raa.ToString()));
    Console.WriteLine("rab="+(rab.ToString()));
    Console.WriteLine("ca="+(ca.ToString()));
    Console.WriteLine("cb="+(cb.ToString()));
    Console.WriteLine("----------------");    
    Console.WriteLine(arithmetics.add(ia,ib));
    Console.WriteLine(arithmetics.add(ia,reb));
    Console.WriteLine(arithmetics.add(ia,rab));
    Console.WriteLine(arithmetics.add(ia,cb));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.add(raa,ib));
    Console.WriteLine(arithmetics.add(raa,reb));
    Console.WriteLine(arithmetics.add(raa,rab));
    Console.WriteLine(arithmetics.add(raa,cb));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.add(rea,ib));
    Console.WriteLine(arithmetics.add(rea,reb));
    Console.WriteLine(arithmetics.add(rea,rab));
    Console.WriteLine(arithmetics.add(rea,cb));
    Console.WriteLine("----------------");
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.add(ca,ib));
    Console.WriteLine(arithmetics.add(ca,reb));
    Console.WriteLine(arithmetics.add(ca,rab));
    Console.WriteLine(arithmetics.add(ca,cb));
    Console.WriteLine("----------------");
  }

  public static void multiply_testing(){
    int a=5,b=20;
    number ia   = new integer(a);
    number rea     = new real(a/1.0);
    number raa = new rational(a,1);
    number ca   = new complex(a,2); 

    number ib   = new integer(b);
    number reb     = new real(b/1.0);
    number rab = new rational(b,1);
    number cb   = new complex(b,15); 
    Console.WriteLine("ia="+(ia.ToString()));
    Console.WriteLine("ib="+(ib.ToString()));
    Console.WriteLine("rea="+(rea.ToString()));
    Console.WriteLine("reb="+(reb.ToString()));
    Console.WriteLine("raa="+(raa.ToString()));
    Console.WriteLine("rab="+(rab.ToString()));
    Console.WriteLine("ca="+(ca.ToString()));
    Console.WriteLine("cb="+(cb.ToString()));
    Console.WriteLine("----------------");    
    Console.WriteLine(arithmetics.multiply(ia,ib));
    Console.WriteLine(arithmetics.multiply(ia,reb));
    Console.WriteLine(arithmetics.multiply(ia,rab));
    Console.WriteLine(arithmetics.multiply(ia,cb));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.multiply(raa,ib));
    Console.WriteLine(arithmetics.multiply(raa,reb));
    Console.WriteLine(arithmetics.multiply(raa,rab));
    Console.WriteLine(arithmetics.multiply(raa,cb));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.multiply(rea,ib));
    Console.WriteLine(arithmetics.multiply(rea,reb));
    Console.WriteLine(arithmetics.multiply(rea,rab));
    Console.WriteLine(arithmetics.multiply(rea,cb));
    Console.WriteLine("----------------");
    Console.WriteLine(arithmetics.multiply(ca,ib));
    Console.WriteLine(arithmetics.multiply(ca,reb));
    Console.WriteLine(arithmetics.multiply(ca,rab));
    Console.WriteLine(arithmetics.multiply(ca,cb));
    Console.WriteLine("----------------");
  }
  
  public static void add_arr_testing(){
    int n=10;
    number[] a = new number[n];
    for (int i=0;i<n;i++){
      a[i]=about_numbers.Randnum();
      Console.WriteLine("+ "+a[i]);
    }
    Console.WriteLine("Sum of arr = "+arithmetics.sum_arr(a));
  } 

}

public class part2a_cs_prog_ass{
  public static void Main() {
    Console.WriteLine("========1.EQUAL========"); 
    testing.equal_testing();
    Console.WriteLine("========2.ADD========"); 
    testing.add_testing();
    Console.WriteLine("========2.Multiply========"); 
    testing.multiply_testing();
    Console.WriteLine("========3.Sum of Array========"); 
    testing.add_arr_testing();


  }
}