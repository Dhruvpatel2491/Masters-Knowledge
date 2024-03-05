"""
Programming Assignment 2 - PART-I
Dhruv Patel
"""

"""
Exercise 3.1: An accumulator is a procedure that is called
repeatedly with a single numeric argument and accumulates its arguments into a sum. 
Each time it is called, it returns the currently accumulated sum. 
Write a procedure make-accumulator that generates accumulators, 
each maintaining an independent sum.
The input to make should specify the initial value of the sum;

"""
print("---------------------------------")
print("=============Ex 3.1==============")
def make_accumulator(intial):
  x = intial
  def inner_accumulator(dx):
    nonlocal x  
    x = x + dx
    return x
  return inner_accumulator

A = make_accumulator(5)
print(A(10))
print(A(10)) 
"""
Exercise 3.2: In sofware-testing applications, it is useful
to be able to count the number of times a given procedure
is called during the course of a computation. 
Write a procedure make-monitored that takes as input a procedure, 
f, that itself takes one input. 
The result returned by make-monitored is a third procedure, say mf, that keeps track
of the number of times it has been called by maintaining
an internal counter. 
If the input to mf is the special symbol
how-many-calls?, then mf returns the value of the counter.
If the input is the special symbol reset-count, then mf resets the counter to zero. 
For any other input, mf returns the result of calling f on that input 
and increments the counter.

"""
print("---------------------------------")
print("=============Ex 3.2==============")
from math import factorial

def make_monitored(f):
    function = f
    counter=0
    def mf(value):
        nonlocal counter
        def how_many_calls():
            return counter
        
        def reset_count():
            nonlocal counter
            counter=0
            
        def mf_interface(input):
            if input == 'how-many-calls?': 
                return how_many_calls()
            elif input == 'reset-count': 
                reset_count() 
                return "Counter RESET"
            else:
                nonlocal counter
                counter+=1             
                   
                return int(function(input))
            
        return mf_interface(value)
    
    return mf

S=make_monitored(factorial)
print(S(5))
print(S(10))
print(S("how-many-calls?"))
print(S("reset-count"))
print(S(6))
print(S("how-many-calls?"))

"""
Exercise 3.3: Modify the make-account procedure so that
it creates password-protected accounts.at is, make-account
should take a symbol as an additional argument, as in
(define acc (make-account 100 'secret-password))

"""

print("---------------------------------")
print("=============Ex 3.3==============")

def make_account_3_3(balance,pwd):
    balance = balance
    sec_pwd=pwd

    def inc_pwd(_):
        return "Incorrect password"
    
    def withdraw(amount):
        nonlocal balance
        if amount>0 and balance>=amount: 
            balance -= amount
            return balance
        else: return "Insufficient funds"
        
    def deposit(amount):
        nonlocal balance
        if amount>0: 
            balance += amount  
            return balance  
    
    def dispatch(pwd,request):
        nonlocal sec_pwd
        if pwd!=sec_pwd : return inc_pwd
        
        if request=="deposit": return deposit 
        elif request=="withdraw": return withdraw
        else:
            print("Unknown request: MAKE-ACCOUNT")
            return None
        
    return dispatch


secret_password="#####"
some_other_password="####$" 

acc = make_account_3_3(100,secret_password)
print(acc(secret_password,"deposit")(40)) 
print(acc(some_other_password,"deposit")(50))

"""
Exercise 3.4: Modify the make-account procedure of Exercise 3.3 by adding another local state variable so that, if
an account is accessed more than seven consecutive times
with an incorrect password, it invokes the procedure call-the-cops
"""
print("---------------------------------")
print("=============Ex 3.4==============")

def make_account_3_4(balance,pwd):
    balance = balance
    sec_pwd=pwd
    security_counter=0
    
    def inc_security_c():
        nonlocal security_counter
        security_counter+=1
        
    def reset_security_c():
        nonlocal security_counter
        security_counter=0
    
    def call_the_cops(use_less):
        return "SECURITY BREACH!!!! CALLING THE COPS"
    
    def inc_pwd(use_less):
        return "Incorrect password"
    
    def withdraw(amount):
        nonlocal balance
        if amount>0 and balance>=amount: 
            balance -= amount
            return balance
        else: return "Insufficient funds"
        
    def deposit(amount):
        nonlocal balance
        if amount>0: 
            balance += amount  
            return balance  
    
    def dispatch(pwd,request):
        nonlocal sec_pwd
        nonlocal security_counter
        if pwd!=sec_pwd : 
            
            inc_security_c()
            if security_counter>7:
                return call_the_cops
            else:
                return inc_pwd
        
        if request=="deposit": return deposit 
        elif request=="withdraw": return withdraw
        else:
            print("Unknown request: MAKE-ACCOUNT")
            return None
        
    return dispatch

secret_password="#####"
some_other_password="####$" 

acc = make_account_3_4(100,secret_password)
print("Correct Calls")
for i in range(1,10):
    response=acc(secret_password,"deposit")(i*10)
    print(f"Correct PWD Call {i} Output:\t{response}") 

print("\nWrong Calls")
for i in range(0,10):
    response=acc(some_other_password,"deposit")(i*10)
    print(f"Wrong PWD Call {i} Output:\t{response}") 

print("\nAgain Correct Calls")
for i in range(1,3):
    response=acc(secret_password,"deposit")(i*10)
    print(f"Correct PWD Call {i} Output:\t{response}") 
"""
Exercise 3.7
"""
print("---------------------------------")
print("=============Ex 3.7==============")

def make_account_3_7(balance,pwd):
    balance = balance
    sec_pwd=pwd

    def inc_pwd(_):
        return "Incorrect password"
    
    def verify_isCorectPwd(input_pwd):
        nonlocal sec_pwd
        return input_pwd==sec_pwd
    
    def withdraw(amount):
        nonlocal balance
        if amount>0 and balance>=amount: 
            balance -= amount
            return balance
        else: return "Insufficient funds"
        
    def deposit(amount):
        nonlocal balance
        if amount>0: 
            balance += amount  
            return balance  
    
    def dispatch(pwd,request):
        nonlocal sec_pwd
        if not verify_isCorectPwd(pwd): 
            return inc_pwd
        
        if request=="deposit": return deposit 
        elif request=="withdraw": return withdraw
        else:
            print("Unknown request: MAKE-ACCOUNT")
            return None
        
    return dispatch

def make_joint(f_owner_acc,f_owner_secret_key,new_joint_sec_key):
    
    def inc_pwd(_): return "Incorrect password"
    

    def make_joint_dispatch(pwd,request):
        if (
            f_owner_acc(f_owner_secret_key,request) == inc_pwd 
            and 
            f_owner_acc(new_joint_sec_key, request) == inc_pwd
            ): 
                return inc_pwd
        
        if f_owner_acc(f_owner_secret_key,request) != inc_pwd:
            return f_owner_acc(f_owner_secret_key,request)

        if f_owner_acc(new_joint_sec_key,request) != inc_pwd:
            return f_owner_acc(new_joint_sec_key,request)

    return make_joint_dispatch       
    

peter_acc = make_account_3_7(100, 'open-sesame')
print(peter_acc('open-sesame', 'deposit')(0)) 
print(peter_acc('open-sesame', 'deposit')(150)) 
paul_acc = make_joint(peter_acc, 'open-sesame', 'rosebud')
print(peter_acc('open-sesame', 'deposit')(50))
print(paul_acc('rosebud', 'deposit')(100))  
print(paul_acc('rosebud', 'withdraw')(30))  

