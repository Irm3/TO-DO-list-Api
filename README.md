TO-DO list Api

API endpoints:

1. Login (POST)
/api/Main/login
Input: "Email", "Password"
 
2. Register (POST)
/api/Main/register
Input: "Email", "Password"(min 12 char)
 
3. Test login auth (GET)
/api/Main/testlogin
 
4. Test admin auth (GET)
/api/Main/testadmin
 
5. Forgot password with known email (POST)
/api/Main/forgot_password/{input email here}

6. Reset password with given token (POST)
/api/Main/reset_password/{token}
Input: "Password", "ConfirmPassword"
 
7. Get all users tasks as admin (GET)
/api/ToDoLists/GetAllUserTasks
  
8. Get current user tasks (GET)
/api/ToDoLists/GetAllMyTasks
 
9. Get task by id as admin (GET)
/api/ToDoLists/Task/{id}
 
10. Edit user task by id (PUT)
/api/ToDoLists/EditMyTask/{id}
Input: "Name", "Status"
 
11. Add task (POST)
/api/ToDoLists/AddTask
Input: "Name", "Status"
 
12. Delete tasks as admin (DELETE)
/api/ToDoLists/DeleteTask/{id}
    
13. Delete current user task (DELETE)
/api/ToDoLists/DeleteMyTask/{id}
