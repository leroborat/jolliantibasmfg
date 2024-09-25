;#DropDB JolliantProdEasyTest

#Application JolliantProd.Win
#Application JolliantProd.Web

*FillForm
 User Name = Admin
*Action Log In

#IfDef JolliantProd.Win

*Action About...
 
*Action OK

#EndIf

#IfDef JolliantProd.Web
*Action Navigation(My Details)
#EndIf
