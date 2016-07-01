#SimpleRL
The C# library for writing RogueLike games

##API
todo

##Examples
All example projects use E?M? naming. Here E? - group and M? - project number in group.
  
E1 - base examples  
E2 - game examples  
E3 - UI examples  
 
### E1 - base examples

E1M1 - Hello world  
![image](https://cloud.githubusercontent.com/assets/1793147/16513381/5d0f37c4-3f75-11e6-9e4a-0b95442d4cd2.png)

You can control console window via static members of **Util** class:  

    Util.Title = "E1M1 - Hello world";
    Util.Width = 80;
    Util.Height = 25;
    Util.CursorVisible = false;

For to control console's content you must use **Util.Buffer**:

    Util.Buffer.Clear();
    Util.Buffer.Write(0, 0, "Hello world");

This library uses buffering, so after **Util.Buffer** was filled you must call **Util.Swap()** method:

    Util.Swap();
  
E1M2  
![image](https://cloud.githubusercontent.com/assets/1793147/16513409/95c474d0-3f75-11e6-957a-a0704232afc0.png)
  
E1M3  
![image](https://cloud.githubusercontent.com/assets/1793147/16513424/bcfcb0da-3f75-11e6-9efd-9addfc320cd0.png)
  
E1M4  
![image](https://cloud.githubusercontent.com/assets/1793147/16513451/f10fc98e-3f75-11e6-8d8c-06663e488caf.png)
  
E1M5  
![image](https://cloud.githubusercontent.com/assets/1793147/16513472/1c963750-3f76-11e6-9056-f0dd725ceedd.png)
  
E1M6  
![image](https://cloud.githubusercontent.com/assets/1793147/16513482/387ec072-3f76-11e6-9910-700bb5a13566.png)
  
### E2 - game examples
  
E2M1  
![image](https://cloud.githubusercontent.com/assets/1793147/16513502/56926622-3f76-11e6-9683-9dcc2da2ddae.png)
  
E2M2  
![image](https://cloud.githubusercontent.com/assets/1793147/16513508/6b360084-3f76-11e6-9004-deea0b9422ca.png)
  
E2M3  
![image](https://cloud.githubusercontent.com/assets/1793147/16513526/92177944-3f76-11e6-8ef8-26a05e3eb600.png)
  
E2M4  
![image](https://cloud.githubusercontent.com/assets/1793147/16513567/cb64d318-3f76-11e6-89f7-534eb8b42fda.png)
  
E2M5  
![image](https://cloud.githubusercontent.com/assets/1793147/16513582/e9d47a1a-3f76-11e6-914d-627d0d458154.png)
  
### E3 - UI examples
  
E3M1  
![image](https://cloud.githubusercontent.com/assets/1793147/16513610/157ba814-3f77-11e6-8536-7d2a0fd57e32.png)
  
E3M2  
![image](https://cloud.githubusercontent.com/assets/1793147/16513620/3347a5c8-3f77-11e6-9d5b-518931867b7a.png)
  
E3M3  
![image](https://cloud.githubusercontent.com/assets/1793147/16513625/43dde12c-3f77-11e6-8f85-04371270c3f0.png)
  
E3M4  
![image](https://cloud.githubusercontent.com/assets/1793147/16513634/53686a40-3f77-11e6-8153-44b4384ccfeb.png)
  
