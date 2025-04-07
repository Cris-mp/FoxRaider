# 🦊 Fox Raider 
En un mundo antiguo y místico, donde la naturaleza esconde templos olvidados, reliquias sagradas y secretos milenarios, vive Foxy, un astuto y ágil zorro aventurero. Durante siglos, los Guardianes del Bosque han protegido el Corazón de Gaia, una gema de inmenso poder que mantiene el equilibrio entre la luz y la oscuridad.  
Sin embargo, una fuerza maligna conocida como Umbra, una sombra ancestral que devora la vida, ha despertado de su letargo y robado la gema, sumiendo los bosques en el caos. Con su hogar en peligro, Foxy emprende una misión épica para recuperarla antes de que sea demasiado tarde.   
Para detener a Umbra, Foxy deberá recorrer dos bosques legendarios:

### 🌳 Bosque Esmeralda   
Un bosque vibrante lleno de vida, pero ahora infestado de criaturas corrompidas. Foxy deberá enfrentarse a zarigüeyas furiosas, murciélagos y evitar trampas ocultas.  
### 🏞️ Bosque Sombrío  
Un paraje pantanoso y peligroso, donde la naturaleza se ha vuelto hostil. Criaturas como las ranas venenosas y fantasmas saltan de las sombras para detenerlo.  

Foxy deberá recorrer ambos bosques, recuperar el Corazón de Gaia y devolver la paz a su hogar, antes de que Umbra lo consuma todo.

# 📖 Diario de proyecto
## 24/03/2025: 🚀 Inicio del proyecto
Inicio del proyecto Fox Raider con la creacion del archivo en Godot y la configuracion de VSCode. Además, se ha creado este repositorio y se ha generado este readme para llevar un control del mismo.
## 28/03/2025: 🦊 Migracion de Foxy (Movimiento y Assets)
Se ha realizado un un esquema de carpetas inicial para organizar las distintas partes del proyecto. Ademas se han agregado las escenas y scripts iniciales, incluyendo camara y niveles del bosque.  
Se siguieron varios tutoriales de internet para realizar y comprender el motor. Para entender los conceptos basicos se visualizo el tutorial: [Cómo usar Godot y Aprender desde CERO a hacer juegos](https://www.youtube.com/watch?v=-_LiMyZGoXw).

### Configuracion de proyecto e Inputs
Se realizo una configuracion del proyecto siguiendo este tutorial: [Crea un PLATAFORMAS 2D con GODOT 4](https://www.youtube.com/watch?v=FORvIiwBXEg&t=1816s), y se añadieron los input a teclado y mando. 

#### 🎮 Configuración de Inputs en Godot
|Nombre | Teclado | Mando|
|-------|---------|------|
|"move_right" | D  | Joystick derecho |
|"move_left" | A  | Joystick izquierdo |
|"jump" | Espacio  | Botón A |
|"roll" | K  | Botón B |

### Animaciones:
Se agregaron animaciones que incluyen las de saltar, caer, rodar, agarrarse a la pared, y caminar. El sprite de Foxy cambia en función del estado de movimiento.
La animación de caída se activa cuando Foxy está en el aire (ni en el suelo ni pegado a la pared) y la velocidad de y es negativa.
### Movimiento y Salto
Se implementó salto y doble salto, permitiendo a Foxy saltar en el suelo o mientras se agarra a una pared (🐛 La parte de salto contra la pared tiene algunos defectos aun).
### Manejo de Paredes
Se permitió que Foxy se agarre a las paredes, pero sin escalar automáticamente. El jugador tiene que moverlo hacia arriba (subir lentamente) o dejarlo caer (🐛 Esta parte tengo que controlarla mejor).
### Otras funciones
Tambien se ha implementado el manejo de la muerte y del daño (Foxy tiene un sistema de salud y muere si se cae demasiado bajo o recibe suficiente daño). Ademas del desbloqueo de habilidades aunque estas funciones necesitan mas desarrollo ya que aun no las he probado en profundidad 🚧.
### Camara
Se configuro una camara para que siga el movimiento del personaje de forma optima.
### Configuracion de Tileset
Se realizó una configuracion basica del tileset del bosque1 para poder hacer las pruebas iniciales del personaje. Se colocaron colisiones y tambien se realizaron autotiles de un par de estructuras. Para ello se siguio el tutorial: [¡TILEMAP, Sistema de TILES 2D!](https://www.youtube.com/watch?v=XVSbjqjJhUQ&list=PL5PTqiCIVoiVyA2qed1NE4uKejXEWM60e&index=14)

## 07/04/2025:🌼Diseño de nivel 1 y corrección de errores

### Corrección de errores
Se resolvieron los problemas que habia con el salto al estar agarrado a la pared y que se deslice lentamente cuando esta agarrado a la pared. Tambien se implemento correctamente el rodar, ahora al rodar pasa por lugares mas estrechos que andando.

### Diseño del primer nivel: Pradera de los susurros
Se realizo el diseño de primer nivel y se implemento parallax en el fondo.

### ➡️ Siguientes pasos
🚧 Acabar la migracion de los Tilemaps (Falta el tilemap de pantano).  
➡️ Migrar el enemigo zarigueya.

# 📊 Planificacion Temporal
✅ Configuración de Godot  
✅ Migrar Movimiento de Foxy  
⬜ Migrar TileMaps y Física  
⬜ IA básica (Zarigüeya)  
⬜ Diseñar los 6 niveles: 1️⃣  
⬜ Programación de enemigos nuevos  
⬜ Desarrollo trampas e interruptores  
⬜ Gestión de salas secretas  
⬜ Sistema de habilidades desbloqueables  
⬜ Sistema de Guardado (JSON)  
⬜ Crear el Selector de Niveles  
⬜ Añadir sonido y efectos  
⬜ Pruebas y ajustes finales  
⬜ Informe Final  

