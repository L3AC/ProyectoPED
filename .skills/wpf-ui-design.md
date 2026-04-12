# WPF UI Design Guide

## Propósito
Esta skill guía el diseño de interfaces atractivas y fluidas en WPF (Windows Presentation Foundation) usando tanto técnicas nativas como librerías externas.

## Contextos de Aplicación
- Proyectos WPF con .NET (Framework o Core)
- Aplicaciones de escritorio Windows
- Interfaces empresariales y de consumo

---

## 1. Principios de Diseño Nativo WPF

### 1.1 Sistema de Colores
- Usar ResourceDictionary para paletas de colores centralizadas
- Definir brushes en App.xaml para consistencia
- Preferir colores con canal alfa para superposiciones
- Usar gradientes sutiles para profundidad visual

```xml
<Application.Resources>
    <SolidColorBrush x:Key="PrimaryBrush" Color="#6200EE"/>
    <SolidColorBrush x:Key="SecondaryBrush" Color="#03DAC6"/>
    <SolidColorBrush x:Key="BackgroundBrush" Color="#F5F5F5"/>
    <SolidColorBrush x:Key="SurfaceBrush" Color="#FFFFFF"/>
    <SolidColorBrush x:Key="TextPrimaryBrush" Color="#212121"/>
    <SolidColorBrush x:Key="TextSecondaryBrush" Color="#757575"/>
</Application.Resources>
```

### 1.2 Tipografía
- Limitar a 2-3 fuentes: una para títulos, otra para cuerpo
- Usar tamaños consistentes: 12sp (caption), 14sp (body), 18sp (subtitle), 24sp (title), 32sp (headline)
- Implementar estilos de texto reutilizables

```xml
<Style x:Key="TitleStyle" TargetType="TextBlock">
    <Setter Property="FontSize" Value="24"/>
    <Setter Property="FontWeight" Value="SemiBold"/>
    <Setter Property="Foreground" Value="{StaticResource TextPrimaryBrush}"/>
</Style>

<Style x:Key="BodyStyle" TargetType="TextBlock">
    <Setter Property="FontSize" Value="14"/>
    <Setter Property="Foreground" Value="{StaticResource TextSecondaryBrush}"/>
    <Setter Property="TextWrapping" Value="Wrap"/>
</Style>
```

### 1.3 Espaciado y Layout
- Usar Grid para layouts principales (flexible y performant)
- Padding base: 8px (mínimo), 16px (estándar), 24px (secciones)
- Margen entre elementos: múltiplos de 8px
- UniformGrid para tarjetas de igual tamaño

### 1.4 Efectos Visuales Nativos
- Sombras con DropShadowEffect (usar con moderación, blur 10-20px)
- Opacidad animada en hover (0.8 -> 1.0)
- Transiciones suaves con DoubleAnimation (200-300ms)
- Border radius: 4px (botones), 8px (tarjetas)

```xml
<Button Content="Click">
    <Button.Style>
        <Style TargetType="Button">
            <Setter Property="Background" Value="{StaticResource PrimaryBrush}"/>
            <Style.Triggers>
                <Trigger Property="IsMouseOver" Value="True">
                    <Setter Property="Opacity" Value="0.9"/>
                </Trigger>
            </Style.Triggers>
        </Style>
    </Button.Style>
</Button>
```

### 1.5 Componentes Recomendados
| Componente | Uso |
|------------|-----|
| Grid | Layout principal |
| StackPanel | Alineación simple |
| Border | Decoración, tarjetas |
| WrapPanel | Botones fluidos |
| UniformGrid | Catálogos iguales |
| Viewbox | Iconos responsive |

---

## 2. Librerías de Diseño Recomendadas

### 2.1 MaterialDesignThemes
```
dotnet add package MaterialDesignThemes --version 5.1.0
```
- Tema: Material Design de Google
- Paletas predefinidas: DeepPurple, Teal, Pink, Blue
- Iconos: Material Design Icons
- Requiere configurar App.xaml

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <materialDesign:BundledTheme BaseTheme="Light" 
                PrimaryColor="DeepPurple" SecondaryColor="Teal"/>
            <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

**Componentes clave:**
- `materialDesign:Card` - Tarjetas con sombra
- `materialDesign:PackIcon` - Iconos (Kind="Home", "Account", etc.)
- `materialDesign:ColorZone` - Barras de encabezado
- `materialDesign:ElevationAssist.Elevation="Dp2"` - Sombras configurables
- `materialDesign:NavigationListBoxItem` - Items de menú

### 2.2 MahApps.Metro
```
dotnet add package MahApps.Metro --version 2.4.10
```
- Estilo Windows moderno
- Temas claros/oscuros
- Controles Metro: MetroWindow, Tile, HamburgerMenu

### 2.3 HandyControl
```
dotnet add package HandyControl --version 3.5.1
```
- Componentes ricos: NumericUpDown, DatePicker, DataGrid mejorado
- Soporte para temas oscuros
- Buena documentación en chino/inglés

### 2.4 Fluent.Ribbon
```
dotnet add package Fluent.Ribbon --version 10.0.4
```
- Barra de cinta estilo Office
- Tabs, grupos, botones rápidos
- Backstage menu

---

## 3. Patrones de Diseño Comunes

### 3.1 Dashboard/Panel de Control
```
┌──────────┬─────────────────────────────────┐
│          │         Header Bar               │
│  Sidebar │─────────────────────────────────│
│  Nav     │  [Card] [Card] [Card] [Card]    │
│          │                                 │
│  - Home  │  Quick Actions:                │
│  - Users │  [Btn] [Btn] [Btn] [Btn]        │
│  - Data  │                                 │
│  - Stats │  Content Area...                │
│          │                                 │
└──────────┴─────────────────────────────────┘
```

### 3.2 Formulario Maestro-Detalle
```
┌─────────────────┬───────────────────────────┐
│   Lista         │   Detalle/Formulario       │
│   ───────       │   ─────────────────────    │
│   > Item 1      │   Campo: [________]        │
│     Item 2      │   Campo: [________]        │
│     Item 3      │                           │
│                 │   [Guardar] [Cancelar]    │
└─────────────────┴───────────────────────────┘
```

### 3.3 Diálogos Modales
- Usar Window con `WindowStartupLocation="CenterOwner"`
- Overlay semitransparente (#80000000)
- Botones de acción alineados a la derecha

---

## 4. Animaciones y Transiciones

### 4.1 Fade In/Out
```xml
<Window.Triggers>
    <EventTrigger RoutedEvent="Window.Loaded">
        <BeginStoryboard>
            <Storyboard>
                <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    From="0" To="1" Duration="0:0:0.3"/>
            </Storyboard>
        </BeginStoryboard>
    </EventTrigger>
</Window.Triggers>
```

### 4.2 Hover Effects
```xml
<Style x:Key="CardHoverStyle" TargetType="Border">
    <Style.Triggers>
        <Trigger Property="IsMouseOver" Value="True">
            <Setter Property="RenderTransform">
                <Setter.Value>
                    <ScaleTransform ScaleX="1.02" ScaleY="1.02"/>
                </Setter.Value>
            </Setter>
            <Setter Property="RenderTransformOrigin" Value="0.5,0.5"/>
        </Trigger>
    </Style.Triggers>
</Style>
```

### 4.3 Page Transitions
Usar `Frame` con `NavigationWindow` o implementar transiciones con `DoubleAnimation` al cambiar páginas.

---

## 5. Mejores Prácticas

### DO ✓
- Centralizar estilos en App.xaml o ResourceDictionary dedicado
- Usar MVVM para separar lógica de vista
- Crear UserControls para componentes reutilizables
- Mantener XAML legible con comentarios de sección
- Usar `x:Name` descriptivo (btnSubmit, txtFirstName)
- Definir tamaños fijos para elementos de navegación

### DON'T ✗
- No hardcodear colores en cada elemento
- No mezclar太多的 fuentes o tamaños arbitrarios
- No usar animaciones excesivas (distrae)
- No crear ventanas con más de 3 niveles de anidamiento
- No ignorar el contraste para accesibilidad

---

## 6. Workflow Recomendado

1. **Definir paleta**: Colores primarios, secundarios, fondo, texto
2. **Crear estilos base**: Buttons, TextBlocks, TextBox
3. **Diseñar layout principal**: Grid con sidebar + contenido
4. **Agregar componentes**: Cards, listas, formularios
5. **Aplicar animaciones**: Transiciones sutiles
6. **Testear**: Diferentes tamaños de ventana, tema del sistema

---

## 7. Comandos Útiles

```bash
# Agregar librería
dotnet add package <PackageName> --version <Version>

# Build
dotnet build

# Run
dotnet run --project <ProjectPath>

# Limpiar build
dotnet clean
```

---

## Recursos
- [WPF Docs Microsoft](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MaterialDesignThemes GitHub](https://github.com/MaterialDesignInXAML/MaterialDesignThemes)
- [MahApps.Metro GitHub](https://github.com/MahApps/MahApps.Metro)
