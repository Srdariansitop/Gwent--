
Gwent++ (Compiler)
=============
Este proyecto es la continuacion de :
`<Link del Primer Proyecto>` : <https://github.com/Srdariansitop/Gwent-Project>

El Reto para este Segundo Proyecto fue interpretar el codigo q el Usuario proporciona y a partir de el crear una Carta q sea compatible con la del juego inicial .

# Diagrama de Flujo del Interprete

 ![Diagrama Funcionamiento](DiagramSystem.png)

 # Carta
- Power : Se permiten numeros o expresiones numericas
 Example : 
 ```javascript
 Power = 6 
 Power = 5 + 2/3 * 8
 ```
- Name : Se permiten cualquier tipo de String (siempre entre comillas)
 Example :
 ```javascript
Name = "Mewtwo"
 ```
- Type : Debido a la constitucion del juego solo se permiten ciertos tipos de cartas (Gold , Silver , Increase , Clime , Leader )
Example :
```javascript
Type = Gold
```
- Range : Igualmente al tener un juego Base solo se permiten lugares expecificos donde invocar la carta en el campo (Meele , Siege , Distance) , se permite mas de un lugar donde invocar la carta siempre entre [] y con separacion entre ,
Example :
```javascript
Range = [Meele , Siege]
```
- Faction : Solo hay dos tipos de Facciones permitidas (Red , Legend)
Example 
```javascript
Faction = Red 
```
>(Todo estos parametros son obligatorios para q el programa puede formar una Carta)

#### Opcional :
+ OnActivaction :
>(Efectos q puedes definir dentro de una partida)

* Selector : 
 * Source : La Fuente donde se pueden deben tomar las Cartas a las q se le aplicaran los efectos  fuentes permitidas son (Deck , OtherDeck , Field , OtherField , Hand , OtherHand) 
 Example :
 ```javascript
 Source = Deck
 ```
 * Single : Un booleano para decidir si me quedo con la primera busqueda en mi Filtro o no (True , False)
Example :
 ```javascript
Single = True
```
  * Predicate : Es el filtro en si por el q van a pasar las cartas del Source 
   Propiedades sobre las cuales se puede filtrar  :
     * unit.power - Se puede comparar con (> , < , >= , <= , == ) ya q al ser entero la                            propiedad Power de las Cartas se puede jugar con todo esto.
      * unit.faction - Con las Facciones ya existentes (Red , Legend)
      * unit.type - Con las Tipos ya existentes (Gold , Silver , Increase , Clime , Leader)
     *   unit.range - Con los espacios en el campo ya existentes (Meele , Siege , Distance )
> (Solo comparacion mediante el ==)

 ```javascript
Example :
Predicate (unit) => unit.power <= 500
```
- PostAction 
>(Efecto que se ejeuta posterior ) :

 *  Type : Tiene q ser un nombre de algun efecto q hayas creado previamente 
  Example :
   ```javascript
  Type = "Draw"
  ```
 * Selector 
 > (Funciona Igualmente q el del OnActivaction con una pequena diferencia en el q el Source puede tomar otro tipo de Fuente (Parent q es una fuente q toma la lista anterior formada en el OnActivaction))

## Example Card Complete  :
   ```javascript
Card
{
Power = 5 + 2/3 * 8
Name ="Mewtwo" 
Faction = Red 
Range = [Meele , Siege] 
Type = Gold 
OnActivation =
[
 Effect
{
Name = "Damage"
}
Selector
{
  Source = Deck
  Single = True
  Predicate (unit) => unit.power <= 500
}
PostAction
{
  Type = "Draw"
  Selector
{
  Source = parent
  Single = False
  Predicate (unit) => unit.faction == Red
}
}
]
}
  ```

# Efecto :

- Name : Cualquier string es permitido , siempre entre " " 
Ejemplo :
```javascript
Name = "Draw"
```
- Params : Los parametros del efecto , se declara entre llaves y luego ponemos las variables que queremos q sean nuestros parametros , siempre igualados al tipo que queremos q sea la variable :
Ejemplo : 
```javascript
Params
{
  amount = Number
}
```
> amount es la variable , y los Tipos permitidos son String , Number , Bool .

- Action
Es el efecto a ejecutar en si , el siguiete codigo es el permitido :

 - Declaraciones de Ciclos :
    - For :

 ```javascript
for Target in Targets
  {
   //Cuerpo de la Funcion
  }
  ```
>  Se permite solo iterar sobre los objectivos

   -   While :
    
   ```javascript
      p = 0;
      while( p++  <  5)
       //Cuerpo de la funcion
   ```
> Iterar sobre una condicion booleana , siempre entre tipos numericos(ya sea entre variables o numeros ) , se acepta comparaciones ( == , < , > , >= , <=)

 ### Extras :

 -  Declaracion de variables :
 > Se acepta cualquier declaracion de cualquier tipo (String , Bool , Number, Var = Var , Var = Cartas o Listas de Cartas , Propiedades de Cartas)

 Ejemplo :
   ```javascript
       i = 0 ;
  ```
- Propiedades de Cartas:
    - target.Power - Modificacion del Poder
   - target.Owner - Identificador de la Card
   - target.Faction - Faccion
   - target.Type - Tipo
  - target.Name - Nombre
>(Tenga en cuenta q la modificacion de una carta en estas propiedades pueden verse afectadas en el juego original)

   Ejemplo  :
   ```javascript
      target.Power -= 100;
   ```
> Se acepta para modificar los operadores ( -= , == , += solo para power , los demas ==) 

 - Declaracion de Listas o Cartas (Indexando) :

   - 1 - La forma mas simple de declararla es como 
        >context.Hand - La mano del usuario actual
		 context.Deck - El deck del usuario actual
	    context.Board - El campo entero

Ejemplo
  ```javascript
    var = context.Deck;
  ```

   -  2 - Atraves del Owner o TriggerPlayer
   
      >context.HandOfPlayer - Deck 
    context.FieldOfPlayer - Campo
    context.DeckOfPlayer - Deck

       Se declara  :
   ```javascript
   lista = context.DeckOfPlayer(target.Owner);
  ```
  
   > Significa el ID del target
   ```javascript
    lista = context.DeckOfPlayer(context.TriggerPlayer);
   ```
 >Significa el ID del q se desencadeno el efecto

   - 3 -_Indexado en Listas :
```javascript
card = context.DeckOfPlayer(context.TriggerPlayer)[1];
  ```
 > Exactamente igual con la diferencia de agregar el indice de la carta en la lista


 -  Metodos:
 
   - Push : Agregar carta al tope de la lista
Ejemplo :
```javascript
context.Hand.Push(card);
  ```
  - SendBottom : Agrega una carta al fondo de la lista
Ejemplo :
```javascript
context.Hand.SendBootom(card);
  ```
  - Pop : Quita la carta q esta en el tope y la devuelvo
Ejemplo :
```javascript
topcard = context.Hand.Pop();
  ```
  
  - Add : Agrega una carta a la lista
Ejemplo :
```javascript
context.Hand.Add(card);
  ```

   - Remove : Quita una carta de la lista 
Ejemplo :
```javascript
context.Hand.Remove(card);
  ```
  - Shuffle: Mezcla la lista
Ejemplo :
```javascript
context.Hand.Shuffle();
  ```
  
   - Find : Devuelve una lista dada una condicion

> Aceptamos en el programa comparaciones con las siguientes propiedades de la carta

   -  unit.power - Se aceptan operadores de comparacion(== , < , > , <= ,>=)
   -  unit.faction - Se acepta solamente como operador de comparacion ==
   - unit.type -  Se acepta solamente como operador de comparacion ==
   - unit.range -  Se acepta solamente como operador de comparacion ==
Ejemplo : 
```javascript
var = context.Hand.Find((unit) => unit.Power == 500);
  ```
> Todos los ejemplos anteriores solo fue usando a Hand como Fuente pero se aceptan fuentes como Deck.

## Codigo Ejemplo de un efecto completo :
```javascript
Effect
{
 Name = "Damage"
Params
{
  amount = Number
}
Action(Targets,Context)=>
{
i = 0;
while(i++ < amount)
target.Power -= 100;
} 
}
  ```
