using System;


  // 1. Write a function

  //    public static bool equals(number a, number b)

  //  that returns true if two "numbers" are equal.  For example:
  //  a rational(1,5) is "equal" to a real(0.2).  However, when making
  //  calculations avoid loss of precision as much as possible: do not convert
  //  the rational to a double.  Instead, just cross-multiply: 1/5 = 0.2
  //  because 1.0 == 0.2*5.  Your function must work for all cases of number.
  //  You may try to use some of the functions already defined in the abstract
  //  class, or not ...

  //  /////// Basic math facts:

  //   rules of rational arithmetic:
  //   a/b + c/d = (a*d+c*b)/(b*d)
  //   a/b * c/d = (a*c)/(b*d)
  //   a/b==c/d  if a*d==b*c
  //   an integer n can be cast to a rational as n/1

  //   rules of complex arithmetic:
  //   (a+bi) + (c+di) = (a+c) + (b+d)i
  //   (a+bi) * (c+di) = (ac-bd) + (bc+ad)i  
  //   a+bi == c+di  if  a==c and b==d.  Complex inequalities don't exit.
  //   (So -1+0i * c+di = -c + -1*i = -c-i)
  //   A real number r can be cast to a complex r+0i.

  //   Note that a complex number can be "equal" to another type of number
  //   only if its imaginary part is zero.

public class arithmetics{
public static bool equals(number a, number b)
{

    if (a is real) 
      a=a.upcast(); // A => Complex
    else if (a is integer)
      a=a.upcast(); // A => Rational

    if (b is real) 
      b=b.upcast(); // B =>Complex
    else if (b is integer)
      b=b.upcast(); // B=> Rational


    if (a is null || b is null) return false;


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


}



public class part2a_cs_prog_ass{
  public static void Main() {
    int n=5;
    number i   = new integer(n);
    number re     = new real(n/1.0);
    number ra = new rational(n,1);
    number c   = new complex(n,1); 

    // Console.WriteLine();
    Console.WriteLine(arithmetics.equals(i,i));
    Console.WriteLine(arithmetics.equals(i,re));
    Console.WriteLine(arithmetics.equals(i,ra));
    Console.WriteLine(arithmetics.equals(i,c));
Console.WriteLine("================");

    Console.WriteLine(arithmetics.equals(ra,i));
    Console.WriteLine(arithmetics.equals(ra,re));
    Console.WriteLine(arithmetics.equals(ra,ra));
    Console.WriteLine(arithmetics.equals(ra,c));

Console.WriteLine("================");
    Console.WriteLine(arithmetics.equals(re,i));
    Console.WriteLine(arithmetics.equals(re,re));
    Console.WriteLine(arithmetics.equals(re,ra));
    Console.WriteLine(arithmetics.equals(re,c));

Console.WriteLine("================");

    Console.WriteLine(arithmetics.equals(c,i));
    Console.WriteLine(arithmetics.equals(c,re));
    Console.WriteLine(arithmetics.equals(c,ra));
    Console.WriteLine(arithmetics.equals(c,c));

  }
}