# BlogProject
ASP.NET Application

Aby zapewnić poprawność działania aplikacji, w pierwszym kroku należy zmienić **ConnectionString** w pliku appsettings.json
* DATA SOURCE={Nazwa maszyny};Integrated Security=true;DATABASE={Nazwa bazy};Trusted_Connection=True;MultipleActiveResultSets=True

</br>

Na przykład: 
* "Server=(localdb)\\MSSQLLocalDB; Database=MyBlog; Trusted_connection=true; MultipleActiveResultSets=true"

---

W kolejnym kroku należy utworzyć bazę danych za pomocą komend:
* dotnet ef migrations add InitialCreate
* dotnet ef database update 

</br>

Po uruchomieniu tych komend w np. Menadżerze MSSQL powinna utworzyć się baza danych

---

Do dyspozycji mamy defaultowo utworzone konto **Administratora**
Jeśli konto **Usera** nie utworzyło się automatycznie należy skorzystać z opcji **Rejestracji**

|               | Admin         |     User      |
| ------------- | ------------- | ------------- |
| Login           |     Admin     | Damian      | 
|  Hasło          | password      | @Damian123  |
