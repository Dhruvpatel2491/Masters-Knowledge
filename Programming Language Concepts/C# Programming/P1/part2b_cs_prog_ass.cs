using System;

public class arithmetics{
    
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
}

public class working{
  public static Option<number> inverse(number n)
  {
    if (n == null)
        return Option<number>.Nothing;

    return n.match(
        I: i =>
            {    if (i.val != 0)
                    return Option<number>.Make(new rational(1,i.val));
                else
                    return Option<number>.Nothing;
            },
        F: f =>
            {
                if (f.n != 0)
                    return Option<number>.Make(new rational(f.d,f.n));
                else
                    return Option<number>.Nothing;
            },
        R: r =>
            {
                if (r.val != 0)
                    return Option<number>.Make(new real(1.0/r.val));
                else
                    return Option<number>.Nothing;
            },
        C: c =>
          {
              if (c.r != 0 || c.i != 0)
              {
                  double s = (c.r*c.r)+(c.i*c.i);
                  return Option<number>.Make(new complex(c.r/s, -c.i/s));
              }
              else
                  return Option<number>.Nothing;   
          }
      );
  }

   public static Option<number> divide(number a,number b) => working.inverse(b).map(invB => arithmetics.multiply(a, invB));

  
}

class testing{

  public static void inverse_testing(){
    int n=8;
    number i  = new integer(n);
    number re = new real(n/2.0);
    number ra = new rational(n,1);
    number c  = new complex(n,2); 
    Console.WriteLine("----------------");    
    Console.WriteLine("I ="+(i));
    Console.WriteLine("Ra ="+(ra));
    Console.WriteLine("Re ="+(re));
    Console.WriteLine("C ="+(c));
    Console.WriteLine("----------------");    
    Console.WriteLine(working.inverse(i));
    Console.WriteLine(working.inverse(ra));
    Console.WriteLine(working.inverse(re));
    Console.WriteLine(working.inverse(c));
    Console.WriteLine("----------------");    


  }
   public static void mul_inverse_testing(){
    int n=8;
    number I  = new integer(n);
    number re = new real(n/2.0);
    number ra = new rational(n,1);
    number c  = new complex(n,2); 
    Console.WriteLine("----------------");    
    Console.WriteLine("I ="+(I));
    Console.WriteLine("Ra ="+(ra));
    Console.WriteLine("Re ="+(re));
    Console.WriteLine("C ="+(c));
    Console.WriteLine("----------------");    
    Console.WriteLine(working.inverse(I));
    Console.WriteLine(working.inverse(ra));
    Console.WriteLine(working.inverse(re));
    Console.WriteLine(working.inverse(c));
    Console.WriteLine("----------------");  

    number pi = new real(3.1415927);
    number i = new integer(2);
    number f = new rational(1,0);
    working.divide(pi,i)                            // Some(pi/2)
    .map(x => arithmetics.multiply(x,new rational(2,3))) // Some(pi/3)
    .and_then(x => working.divide(x,f))             // None
    .match(some: v => {Console.WriteLine("the result is "+v);},
           none: () => {Console.WriteLine("no result");});
  // note that .map is called on functions number -> number
  // whereas .and_then is called on functions number -> Option<number>
    Console.WriteLine("----------------");  

  }


}

public class exp_hand{
  public static void q61(){
    int n = int.MaxValue;  // or 0x7fffffff, largest 32bit int
           checked {
             n = n*2;
    }
    //Error:-
    /*
    Unhandled Exception:
    System.OverflowException: Arithmetic operation resulted in an overflow.
    at part2b_cs_prog_ass.Main () [0x0000c] in <b4e411bee35e4b319a06eeee82ac7e57>:0 
    [ERROR] FATAL UNHANDLED EXCEPTION: System.OverflowException: Arithmetic operation resulted in an overflow.
    at part2b_cs_prog_ass.Main () [0x0000c] in <b4e411bee35e4b319a06eeee82ac7e57>:0 
    */
  }

  public static Option<number> safemult(Option<number> a, Option<number> b)
{
  return a.match(
      some: aVAL =>
      {
          return b.match(
              some: bVAL =>
              {
                  try
                  {
                      return Option<number>.Make(arithmetics.multiply(aVAL, bVAL));
                  }
                  catch (OverflowException)
                  {
                      return Option<number>.Nothing;
                  }
              },
              none: () => Option<number>.Nothing
          );
      },
      none: () => Option<number>.Nothing
  );
}

public static void q62(){

  var a = Option<number>.Make(new integer(int.MaxValue));
  var ib = Option<number>.Make(new integer(30));
  var b = Option<number>.Make(new rational(3,2));
  var c = exp_hand.safemult(b,b).pair(exp_hand.safemult(a,b), (x,y) => arithmetics.multiply(x,y));
  Console.WriteLine(c); // overflow, but better not crash
  Console.WriteLine(exp_hand.safemult(a,ib)); // overflow, but better not crash
}

}

public class part2b_cs_prog_ass{
  public static void Main() {
    Console.WriteLine("========4.Inverse========"); 
    testing.inverse_testing();
    Console.WriteLine("========5.Multiplicative inverse========"); 
    testing.mul_inverse_testing();
    Console.WriteLine("========6.Exception Handelling========"); 
    // exp_hand.q61(); //Uncomment to see error PartB - Q6 i
    exp_hand.q62();
  }
}