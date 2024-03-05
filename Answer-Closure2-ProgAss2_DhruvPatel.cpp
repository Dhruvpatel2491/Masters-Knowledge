#include<iostream>
#include<cstdio>      /* prefer printf to cout */
#include<string>
#include<functional>  /* requires std=c++14 */
using namespace std;

/*          CLOSURES PROGRAMMING ASSIGNMENT, PART II

 Unlike with languages such as Python and Java, C/C++ programmers must
be keenly aware of where in memory their data are stored, and of their
LIFETIME.  The lifetime of a value is how long it's guaranteed to be
at the same location in memory.  The lifetime ends when, for example,
a value gets popped from the stack at the end of a function call, or
deallocated from the heap.  The value may persist in memory
momentarily after its lifetime ends but is unusable as it will be
overwritten.

Recall that we could not create a proper closure, even an immutable one,
in C, because in

int K(int x) {
  int inner(int y) { return x; }
  return inner;
}

The value x is stored on the stack, and gets popped after K returns.  The 
function returned OUTLIVES its free variable x.  

Languages like Python that allow such closures to be created typically will
copy the free variable x to some location on the HEAP, where they persist 
after the function call.  The heap-allocated data is eventually erased by a 
garbage collector.

But in C/C++ we do not have (and do not want) a garbage collector.  We
therefore want to allocate our data on the stack as much as possible,
to avoid memory errors and the overhead of heap management.  By
default, C++ will always allocate its structures on the stack unless
certain keywords like `new`, `malloc` or `make_unique`, are used to
allocate structures on the heap.  It will NEVER move something to heap
behind the scenes for you.  If there's a version of C++ that does
that, it's actually Java in disguise (and should make you vomit).

"Modern" C++ made huge strides since it was born in 2011 (or 2014 depending
on your perspective), but it stays true to its core principle of ZERO 
OVERHEAD ABSTRACTION.

Lambda terms in modern C++ are different from other languages in that not
only do we have to specify the bound variables, but also how FREE variables
are to be captured.  We have the choice of capturing them by reference (hidden
pointer) or by value. 
*/
auto I = [](auto x) {return x;};  // lambda x.x
auto K = [](auto x){ return [x](auto y) { return x; }; }; //lambda x.lambda y.x
auto S = [](auto x){return [x](auto y){return [x,y](auto z){return x(z)(y(z));};};}; // this is lambda x.lambda y.lambda z.x z (y z)

/*
The 'auto' keyword does an adequate form of type inference.

In the lambda term returned by the K combinator, the notation [x] means that
the free variable x inside the inner lambda y.x is captured by value.
Had we written K this way:
*/
auto badK = [](auto x){ return [&x](auto y) { return x; }; };
/*
then we would have the same problem as the C program.  The free variable x
is captured by REFERENCE, but the value still gets popped from the stack.
The reference outlives the value that it refers to (a dangling reference).

You can also write [=] or [&] to capture all free vars by value, or all by
reference, respectively, or [x,&y,...] to be selective.

A C++ lambda term must be marked `mutable` if it changes any of its
free variables (you can always change the bound variables).  It's when
`mutable` is present that a proper closure is created. In an immutable
closure, such as the function returned by K, free variables captured
by value are replaced by those values, so the term being returned by K(2),
for example, is equivalent to [](auto y){return 2;}.  The free variable
disappeared (this is the only kind of lambda term allowed in Java).  
A "mutable" lambda term, on the other hand, creates a closure by allocating
additional memory for the free variables that it captured.  This memory is
allocated on the STACK.  The values of the free variables are copied to
this structure initially, but can be changed later. 

The following function is the C++ equivalent of the "make_accumulator"
function in Python (and Scheme):
*/
function<int(int)> make_accumulator() {
  int x = 0;
  return [x](int dx) mutable { x += dx; return x; };
}
/*
This time we wrote down the exact types to be clear instead of using `auto`:
function<int(int)> is the type `int -> int`.  The function returned by
make_accumulator is a mutable closure allocated on the stack.  when you do

  auto a1 = make_accumulator(); // see main below

you're binding the stack-allocated structure to the L-value a1.  The
structure with its free variables have the same lifetime as a1.
The closure returned is roughly equivalent to an instance of
*/
struct accumulator {
private:
  int x;
public:
  accumulator():x{0} {}
  int operator ()(int dx) { x+=dx; return x;}
};
/*
By indicating which free variables to capture, how to capture them,
and whether the closure is mutable, you are dictating the contents of
this struct.  If you don't understand the above struct don't worry
about it: just focus on lambda terms for now.  The analogy between
a lambda closure and a struct/class is approximate and not meant as
a direct translation.

We can now try to reproduce the bank account example, except it's not as
easy as we might think.  The following compiles, but DOESN'T REALLY WORK:
*/

auto newaccount = [](string name, double balance) {

  function<double()> balinquiry = [&name,&balance]() {
    printf("%s has a balance of %.2f\n",name.c_str(),balance);
    return balance;
  };
  function<void(double)> deposit = [&balance](double amt) mutable { 
    if (amt>0) balance+=amt;   
  };  // mutable means function could mutate free variables
  function<void(double)> withdraw = [&balance](double amt) mutable {
    if (amt>0 && amt<=balance) balance-=amt;
  };
  function<void(double)> dummyfun = [&balance](double x) mutable {balance+=0;};

  auto public_interface =
    [=](const string& request) {
      if (request=="deposit") {
        return deposit;
      }
      else if (request=="withdraw") {
        return withdraw;
      }
      else if (request=="inquiry") {
        balinquiry();
      }
      return dummyfun;  // this is required for type-consistency
  };
  return public_interface;
};//newaccount

int main() {
  printf("%s\n", K("abc")(9));   // prints abc
  printf("%d\n", K(I)(2)(123));  // prints 123 - K is "polymorphic"

  auto a1 = make_accumulator();
  auto a2 = make_accumulator();
  cout << a1(2) << endl; // prints 2
  cout << a1(2) << endl; // prints 4
  cout << a2(1) << endl; // prints 1  

  auto myaccount = newaccount("Liang",1000);
  auto youraccount = newaccount("Student",2000);
  myaccount("inquiry");      // SHOULD print Liang has a balance of 1000
  youraccount("inquiry");    // SHOULD print Student has a balance of 2000
  myaccount("withdraw")(300);
  myaccount("deposit")(100);
  myaccount("inquiry");      // SHOULD print Liang has a balance of 800
  return 0;
}//main

/*
Compile and run this to look at the output (may have to compile with -std=c++14
option).  Can you explain the output?  What happened?

Note that the public_interface function captured the name and balance
variables by value, but the other closure-functions captured them by reference.

Your first instinct might be to change all the lambda terms to capture by
value.  Try it and see what happens.

The problem is that the withdraw and deposit functions of the same
account MUST capture the balance by reference, otherwise they will
just be modifying their own copies!  They need to change the SAME
balance.  Same goes for the balinquiry method: if it captured balance by
value then it will always return the same value.

Clearly, in order to get this to work sometimes the free vars must be 
captured by reference, while at other times they must be captured by value 
or they'll get popped from the stack.

   ***  YOUR ASSIGNMENT IS TO MAKE THIS PROGRAM WORK CORRECTLY.  ***

Like in part 1 of this assignment, YOU ARE NOT ALLOWED to use any
structs or classes, imported or user-defined (vector, unordered_map,
etc, are all structs/classes that you can't use), other than std::string.
You must write lambda terms to create closures.

Also, you may not make the functions such as withdraw/deposit gobal, or
take addtional arguments.  You can only rearrange how they're defined 
inside the newaccount function...
*/
