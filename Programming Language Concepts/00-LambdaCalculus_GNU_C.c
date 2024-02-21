#include<stdio.h>
#include<stdlib.h>

#define NIL 0
#define BOOL int
#define DIV printf("\n-------------------------\n");

typedef unsigned int uint;  // convenient name for unsigned int

// The following defines the TYPES OF FUNCTIONS
typedef int (*fun)(int);  /* fun is the type of functions int->int */
typedef int (*binop)(int,int); /* binary operation on ints, int*int -> int */
typedef BOOL (*predicate)(int); /* predicates on integers */
typedef void (*action)(); /* simple action, no return value */
typedef void (*actionon)(int); /* act on an integer */

/* refer to programs written in class for more code and examples */

// Basic linked-list encapsulation
typedef struct cell* list;  // linked list pointer
struct cell
{
  int item;
  list next;
};

list cons(int x, list n) // creates struct on heap
{
  list newcell = (list)malloc(sizeof(struct cell));
  newcell->item = x;
  (*newcell).next = n;
  return newcell;
}
struct cell CONS(int x, list n) { // creates actual struct, return on stack
  struct cell newcell; // stack allocated struct
  newcell.item = x;
  newcell.next = n;
  return newcell;
}

// list m = cons(2,cons(3,cons(5,cons(7,NIL))));
int car(list m) { return m->item; }
list cdr(list m) { return m->next; }
int cadr(list m) { return m->next->item; } // second item
list cddr(list m) { return m->next->next; }
BOOL isnil(list m) { return m==NIL; }  //tests for empty list

//// IN YOUR CODE THESE ARE THE ONLY OPERATIONS YOU CAN CALL ON LISTS:F
// cons, car, cdr, isnil and functions defined from them: you may NOT access
// the struct cell encapsulated by these functions (and you won't have to).
// You may not call malloc/calloc and you can't use any pointer notation.

// sample functions on lists (similar to lecture examples).
list reverse(list m)
{
  // need a stack, initially null
  list irev(list m, list stack)
  {
    if (isnil(m)) return stack; else return irev(cdr(m), cons(car(m),stack));
  }
  return irev(m,NIL);
}

list map(fun f, list m)
{
  list imap(list m, list stack)
  {
    if (isnil(m)) return stack;
    else return imap(cdr(m), cons(f(car(m)),stack));
  }
  return reverse(imap(m,NIL));
}

/* in scheme
(define (reduce bop id m)
  (define (inner m ax) (if (null? m) ax (inner (cdr m) (bop ax (car m)))))
  (if (null? m) id (inner (cdr m) (car m))))
(reduce (lambda (x y) (+ x y)) 0 '(3 4 5 1))
*/
// the id is the "left-identity"
int reduce(binop bop, int id, list m)  
{
  int inner(list m, int ax) {
    if (isnil(m)) return ax; else return inner(cdr(m),bop(ax,car(m)));
  }
  if (isnil(m)) return id; else return inner(cdr(m),car(m));
}// repeated apply bop to all values of list, returning id for empty list

/* there-exists:
(define (exists p m)   
  (if (null? m) #f 
     (if (p (car m)) #t (exists p (cdr m)))))
*/
BOOL exists(predicate p, list m)
{
  return m!=NIL && (p(car(m)) || exists(p,cdr(m)));
}// is this tail recursive? TRICK QUESTION

BOOL forall(predicate p, list m)
{
  BOOL notp(int x) { return !p(x); }
  return !exists(notp,m);  //forall x.P(x) == !exists x.!P(x)
}

int main(){
// Testing LIST
list m=cons(1,cons(3,cons(4,cons(9,cons(12,cons(25,cons(26,NIL)))))));

// Print Function to Display
int print (int x){ printf("%d ",x); return 0;};
printf("Initial LIST M:");
map(print,m);
DIV
/*1. write the equivalent of the following scheme function, which "doubles up" each value of a list.
(define (doubleup m)
   (if (null? m) m (cons (car m) (cons (car m) (doubleup (cdr m))))))

for example, (doubleup '(1 2 3)) returns '(1 1 2 2 3 3)).  
This function is recursive but not tail-recursive.  ALL FUNCTIONS AFTER #1 must be tail-recursive.
*/
list doubleup(list m)
{
  list doubling(int i, list rest){ return cons(i,cons(i,doubleup(rest))); };

  if(isnil(m))  return m; 
  else          return doubling(car(m), cdr(m)); 
};
list du_1A = doubleup(m);
printf("Q1A: doubleup NO Tail recursion\n");
map(print,du_1A);
DIV
/*
1b. Write a tail-recursive version of the doubleup function: wrap the tail
   recursive function inside an outer function.  You may also want to copy
   over the reverse function from above.
*/
list doubleup_tailREC(list m)
{
  list doubling_inner(list m) 
  {
    if (isnil(m)) 
      return m;
    else 
      return cons(car(m),cons(car(m),doubling_inner(cdr(m))));
  };
  return doubling_inner(m); 
};
list du_1B = doubleup_tailREC(m);
printf("Q1B: doubleup WITH Tail recursion\n");
map(print,du_1B);
DIV

/*
2. define a new type:
   typedef void (*actionon)(int);

   Then define a "foreach" loop construct that works as follows:

   void output(int x) { printf("%d ",x); }
   foreach(m,output); // would print every value in m

   It is possible to use map for printing, but that would be a bit
   wasteful because, unlike map, foreach doesn't need to return anything.  
   In Scheme you can define it tail-recursively with

   (define (foreach m f)   ; arguments are list m and function f
      (if (not (null? m)) (begin (f (car m)) (foreach (cdr m) f))))

   The signature (header) of your foreach function should be

   void foreach(list m, actionon f);
   or
   void foreach(list m, void (*f)(int));  // without typedef

*/
typedef void (*actionon)(int);
void output(int x) { printf("%d ",x); };
void for_each(list m, actionon f)
{
  list f_e_inner(list m)
  {
    if (!isnil(m)) 
    { 
      f(car(m));
      f_e_inner(cdr(m));
    };  
  };
  f_e_inner(m); 
};
printf("Q2: For Each + OUTPUT Function \n");
for_each(m,output);
DIV

/*
3. Write a function to print every value in a list using the foreach loop
   you created above for problem 2.  If you're not sure of the above, write a
   tail recursive function to print from scratch first.
*/

void for_each_printf(list m)
{
  list f_e_pf_inner(list m)
  {
    if (!isnil(m)) 
    { 
      printf("%d ",car(m));
      f_e_pf_inner(cdr(m));
    };  
  };
  f_e_pf_inner(m); 
};
printf("Q3: For Each-PRINT  \n");
for_each_printf(m);
DIV

/*
4. write a higher-order function 'howmany' that takes a predicate
   as an argument and returns how many values in a list satisfy the
   given property.  In scheme, the function should behave as follows:

   (howmany (lambda (x) (< x 0)) (cons 2 (cons -3 (cons 4 (cons -1 ())))))
   should return 2 because there are two negative numbers in the list.

   Here's how you'd write the function non-tail recursively in scheme:
   (define (homany p m) 
      (if (null? m) 0 (+ (if (p (car m)) 1 0)+(homany p (cdr m)))));
      (if (p (car m)) 1 0) evaluates to 1 or 0, which is added to (howmany..)
  
   YOUR SOLUTION IN C MUST BE TAIL RECURSIVE.

   hint: for the (inner) tail-recursive function, the counter should be 
   passed in as an extra argument.
*/

int how_many(predicate p,list m)
{
  int howmany_inner(list m,int ctr)
  { 
    if(isnil(m))
      return ctr;
    else
      return howmany_inner(cdr(m),p(car(m))+ctr);
  }
  return howmany_inner(m,0);
}

int isNEG(int x){return x<0;};

printf("Q4: howmany are NEGATIVE \n");
list m4=cons(-1,cons(3,cons(-4,cons(9,cons(-12,cons(25,cons(-26,NIL)))))));
int hm_4=how_many(isNEG,m4);
for_each_printf(m4);
printf("\nIn M4 Positives are = %d",hm_4);
DIV


/*
5. write a higher order function 'filter' that takes a predicate p and list
   m as an arguments and returns a list with just those values in m that
   satisfies the predicate.  For example, (in scheme syntax)

   (filter (lambda (x) (< x 0)) (cons 2 (cons -3 (cons 4 (cons -1 ())))))

   should return a list '(-3 -1), which are the values in the original list
   that are negative. The order of values in the filtered list is not important.
*/

list filter(predicate p,list m)
{
  list filter_inner(list m,list stack)
  { 
    if(isnil(m))
      return stack;
    else{
      if(p(car(m)))
        return filter_inner(cdr(m),cons(car(m),stack));
      else
        return filter_inner(cdr(m),stack);
    }
      
  };
  return filter_inner(m,(list)NIL);
};

BOOL isEVEN(int x){return x%2==0;};

printf("Q5: filter are NEGATIVE\n Before:\n");
for_each_printf(m);
list m_even_list=filter(isEVEN,m);
printf("\nEven List:");
for_each_printf(m_even_list);
DIV

/*
6. write a function that takes two lists as arguments and returns their
   intersection: a list that contains all values found in both lists
   (ordering and duplicates do not matter).  For example, given

   list m = cons(2,cons(4,cons(6,NIL)));
   list n = cons(2,cons(5,cons(6,cons(8,NIL))));
   list mn = intersection(m,n); // should return list cons(2,cons(6,NIL))  

   hint: for the tail-recursive function the intersection to be built (mn)
   should be an extra argument.
*/

list intersection(list m, list n)
{
  list intersection_inner(list m,list n,list mn){
    if(isnil(m))
      return mn;
    else
    {
      BOOL checkIntersect(int x){return x==car(m);};
  
      if(exists(checkIntersect,n))
          return intersection_inner(cdr(m),n,cons(car(m),mn));
      else
        return intersection_inner(cdr(m),n,mn);
        
    }
  }
  return intersection_inner(m,n,NIL);
};

list m6 = cons(2,cons(4,cons(6,NIL)));
list n6 = cons(2,cons(5,cons(6,cons(8,NIL))));

printf("Q6: intersection of 2 list");
printf("\nM :");
for_each_printf(m6);
printf("\nN :");
for_each_printf(n6);
printf("\nMN:");
list mn = intersection(m6,n6);
for_each_printf(mn);
DIV

/*
7a. write a function 'sublist' so that sublist(m,n) returns true if every
   value in m is also found in n (ordering and duplicates don't matter),
   and returns false othewise.  
*/

BOOL sublist(list m, list n)
{
  BOOL sublist_inner(list m,list n,BOOL subStatus){
    if(isnil(m))
      return subStatus;
    else
    {
      BOOL checkIntersect(int x){return x==car(m);};
  
      if(exists(checkIntersect,n))
          return sublist_inner(cdr(m),n,subStatus);
      else
        return 0;
    }
  };
  return sublist_inner(m,n,1);
};

list m7 = cons(2,cons(5,cons(6,NIL)));
list n7 = cons(2,cons(5,cons(6,cons(8,NIL))));

printf("Q7a: sublist of 2 list");
printf("\nM :");
for_each_printf(m7);
printf("\nN :");
for_each_printf(n7);
BOOL sl_7a = sublist(m7,n7);
printf("Is M sublist of N: %d",sl_7a);
DIV

printf(" \n");
return 0;
}
