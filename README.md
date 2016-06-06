NJection
===================

`NJection` provides a way to inject .NET code at runtime using configuration files.<br/>
Through parsing a configuration file an expression tree is built and can be later on compiled and run, thus, giving<br/> thepossibility to inject different pieces of code.<br/><br/>

Please visit the [github pages](https://sagifogel.github.io/NJection/) to see the documentation.

## Parsing

Parsing the configuration file is done by using the ExpressionBuilder's Traverse method:

```c#
LambdaExpression expression = ExpressionBuilder.Traverse<LambdaExpression>(@"C:\Example.config");
Action<string> actionOfString = expression.Compile() as Action<string>;

actionOfString("Hello World!");
```

## Configuration

Here we defined a method call to WriteLine of the static class Console.

```xml
<?xml version="1.0" encoding="utf-8"?>
 <configuration>
  <expression type="Lambda" typeof="System.Action`1[System.String]">
   <expression type="Call" typeof="System.Console"kind="Static" methodName="WriteLine">
    <arguments>
     <expression ref="valueParameter"/>
    </arguments>
   </expression>
   <arguments>
    <expression type="Parameter" typeof="System.String" name"valueParameter"/>
   </arguments>
  <expression>
 <configuration>
```
