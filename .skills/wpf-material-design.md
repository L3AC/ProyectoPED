# WPF Material Design Guide

## Propósito
Guía para implementar interfaces con Material Design (estilo Google) usando MaterialDesignThemes, incluyendo la versión 5.x con Material Design 3.

## Contextos de Aplicación
- Apps que quieren look-and-feel de Google/Android
- Proyectos que requieren componentes ricos pre-construidos
- Interfaces con muchos formularios y dialogs

---

## 1. Instalación y Configuración

### Paquete
```powershell
dotnet add package MaterialDesignThemes --version 5.1.0
```

### Configuración App.xaml
```xml
<Application x:Class="App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:materialDesign="http://schemas.microsoft.com/netfx/2009/xaml/presentation"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Tema con paleta de colores -->
                <materialDesign:BundledTheme BaseTheme="Light" 
                    PrimaryColor="Indigo" 
                    SecondaryColor="Teal"/>
                
                <!-- Recursos base de Material -->
                <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### Paletas de Colores Disponibles
```
Primarios: Red, Pink, Purple, DeepPurple, Indigo, Blue, 
           LightBlue, Cyan, Teal, Green, LightGreen, Lime, 
           Yellow, Amber, Orange, DeepOrange, Brown, Grey, BlueGrey

Secundarios: Mismo listado
```

---

## 2. Tarjetas Material

### Card Básica
```xml
<materialDesign:Card Padding="16" Margin="8">
    <StackPanel>
        <TextBlock Text="Título" 
                   Style="{materialDesign:TextBlockStyle Headline6}"/>
        <TextBlock Text="Contenido de la tarjeta"
                   Style="{materialDesign:TextBlockStyle Body2}"
                   Margin="0,8,0,0"/>
    </StackPanel>
</materialDesign:Card>
```

### Card con Imagen y Acciones
```xml
<materialDesign:Card>
    <materialDesign:Card.Media>
        <Image Source="/Images/landscape.jpg" 
               Height="150"
               Stretch="UniformToFill"/>
    </materialDesign:Card.Media>
    <materialDesign:Card.Actions>
        <StackPanel Orientation="Horizontal" 
                    HorizontalAlignment="Right" 
                    Margin="8">
            <Button Content="COMPARTIR" 
                    Style="{StaticResource MaterialDesignFlatButton}"/>
            <Button Content="MÁS" 
                    Style="{StaticResource MaterialDesignFlatButton}"/>
        </StackPanel>
    </materialDesign:Card.Actions>
</materialDesign:Card>
```

### Card Elevated (con sombra)
```xml
<materialDesign:Card materialDesign:ElevationAssist.Elevation="Dp4">
    <TextBlock Text="Tarjeta elevada"/>
</materialDesign:Card>
```

---

## 3. Botones Material

### Variantes de Botones
```xml
<!-- Contained (Filled) - Primario -->
<Button Content="PRIMARY" 
        Style="{StaticResource MaterialDesignRaisedButton}"/>

<!-- Outlined -->
<Button Content="OUTLINED" 
        Style="{StaticResource MaterialDesignOutlinedButton}"/>

<!-- Text -->
<Button Content="TEXT" 
        Style="{StaticResource MaterialDesignFlatButton}"/>

<!-- Icon Button -->
<Button Style="{StaticResource MaterialDesignIconButton}"
        ToolTip="Favorite">
    <materialDesign:PackIcon Kind="Heart"/>
</Button>

<!-- FAB (Floating Action Button) -->
<Button Style="{StaticResource MaterialDesignFloatingActionButton}"
        materialDesign:ButtonAssist.CornerRadius="28">
    <materialDesign:PackIcon Kind="Plus" Width="24" Height="24"/>
</Button>

<!-- Extended FAB -->
<Button Style="{StaticResource MaterialDesignExtendedFloatingActionButton}"
        Content="AGREGAR"/>
```

### Botones con Iconos
```xml
<StackPanel Orientation="Horizontal">
    <Button Style="{StaticResource MaterialDesignRaisedButton}"
            materialDesign:ButtonAssist.Icon="ContentSave">
        Guardar
    </Button>
    
    <Button Style="{StaticResource MaterialDesignOutlinedButton}"
            materialDesign:ButtonAssist.Icon="Delete"
            Margin="8,0">
        Eliminar
    </Button>
</StackPanel>
```

---

## 4. Iconos Material Design

### PackIcon - Galería Común
```xml
<materialDesign:PackIcon Kind="Home" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Account" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Cog" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Plus" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Check" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Pencil" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Magnify" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Calendar" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="ChartBar" Width="24" Height="24"/>
<materialDesign:PackIcon Kind="Bell" Width="24" Height="24"/>
```

### Cambiar Color de Icono
```xml
<materialDesign:PackIcon Kind="Star" 
                        Foreground="{DynamicResource PrimaryHueMidBrush}"
                        Width="32" Height="32"/>
```

---

## 5. Chips y Badges

### Chips (etiquetas interactivas)
```xml
<StackPanel Orientation="Horizontal">
    <!-- Chip Simple -->
    <materialDesign:Chip Content="Tag 1"/>
    
    <!-- Chip con Icono -->
    <materialDesign:Chip Content="Archivo" 
                          Icon="File">
        <materialDesign:Chip.Icon>
            <materialDesign:PackIcon Kind="File"/>
        </materialDesign:Chip.Icon>
    </materialDesign:Chip>
    
    <!-- Chip deletible -->
    <materialDesign:Chip Content="Deleteme" 
                          IsDeletable="True"/>
    
    <!-- Chip de acción -->
    <materialDesign:Chip Content="Click me"
                          Icon="GestureTap"
                          Click="Chip_Click"/>
</StackPanel>
```

### Badges
```xml
<Grid>
    <materialDesign:PackIcon Kind="Bell" 
                             Width="24" Height="24"/>
    <materialDesign:Badged Badge="3" 
                            BadgeColorZoneMode="PrimaryMid"
                            HorizontalAlignment="Right"
                            VerticalAlignment="Top"
                            Margin="-5"/>
</Grid>
```

---

## 6. Campos de Formulario

### TextBox Material
```xml
<StackPanel Margin="0,0,0,16">
    <TextBox materialDesign:HintAssist.Hint="Nombre"
             materialDesign:HintAssist.Label="Nombre completo"
             Style="{StaticResource MaterialDesignOutlinedTextBox}"/>
    
    <TextBox materialDesign:HintAssist.Hint="Email"
             materialDesign:ValidationAssist.OnlyShowOnFocus="True"
             Style="{StaticResource MaterialDesignOutlinedTextBox}"
             Margin="0,16,0,0"/>
</StackPanel>
```

### PasswordBox
```xml
<PasswordBox materialDesign:HintAssist.Hint="Contraseña"
             materialDesign:TextFieldAssist.HasClearButton="True"
             Style="{StaticResource MaterialDesignOutlinedPasswordBox}"/>
```

### ComboBox
```xml
<ComboBox materialDesign:HintAssist.Hint="Seleccionar"
          Style="{StaticResource MaterialDesignOutlinedComboBox}">
    <ComboBoxItem Content="Opción 1"/>
    <ComboBoxItem Content="Opción 2"/>
    <ComboBoxItem Content="Opción 3"/>
</ComboBox>
```

### DatePicker
```xml
<DatePicker materialDesign:HintAssist.Hint="Fecha"
            Style="{StaticResource MaterialDesignOutlinedDatePicker}"
            Width="200"
            HorizontalAlignment="Left"/>
```

---

## 7. DataGrid Material

```xml
<DataGrid ItemsSource="{Binding Items}"
          AutoGenerateColumns="False"
          CanUserAddRows="False"
          materialDesign:DataGridAssist.CellPadding="13 8 8 8"
          materialDesign:DataGridAssist.ColumnHeaderPadding="8">
    
    <DataGrid.Columns>
        <DataGridTextColumn Header="Nombre" 
                            Binding="{Binding Name}"
                            Width="*"/>
        <DataGridTextColumn Header="Estado" 
                            Binding="{Binding Status}"
                            Width="100"/>
        <DataGridTemplateColumn Header="Acciones" Width="150">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <StackPanel Orientation="Horizontal">
                        <Button Style="{StaticResource MaterialDesignIconButton}"
                                ToolTip="Editar"
                                Tag="{Binding Id}">
                            <materialDesign:PackIcon Kind="Pencil"/>
                        </Button>
                        <Button Style="{StaticResource MaterialDesignIconButton}"
                                ToolTip="Eliminar"
                                Foreground="{DynamicResource MaterialDesignValidationErrorBrush}">
                            <materialDesign:PackIcon Kind="Delete"/>
                        </Button>
                    </StackPanel>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
    </DataGrid.Columns>
</DataGrid>
```

---

## 8. Dialogs y Snackbar

### Dialogo Simple
```xml
<materialDesign:DialogHost Identifier="RootDialog">
    <materialDesign:DialogHost.DialogContent>
        <StackPanel Margin="24" MinWidth="300">
            <TextBlock Text="Confirmar acción" 
                       Style="{materialDesign:TextBlockStyle Headline6}"/>
            <TextBlock Text="¿Está seguro de continuar?" 
                       Margin="0,16,0,24"/>
            <StackPanel Orientation="Horizontal" 
                        HorizontalAlignment="Right">
                <Button Content="CANCELAR"
                        Style="{StaticResource MaterialDesignFlatButton}"
                        Command="{x:Static materialDesign:DialogHost.CloseDialogCommand}"/>
                <Button Content="ACEPTAR"
                        Style="{StaticResource MaterialDesignFlatButton}"
                        Command="{x:Static materialDesign:DialogHost.CloseDialogCommand}"/>
            </StackPanel>
        </StackPanel>
    </materialDesign:DialogHost.DialogContent>
    
    <!-- Contenido principal -->
    <Grid>
        <Button Content="Abrir Dialog"
                Command="{x:Static materialDesign:DialogHost.OpenDialogCommand}"/>
    </Grid>
</materialDesign:DialogHost>
```

### Snackbar (notificaciones)
```xml
<materialDesign:Snackbar x:Name="MySnackbar" 
                          MessageQueue="{materialDesign:MessageQueue}"/>

<!-- En código -->
MySnackbar.MessageQueue.Enqueue("Guardado exitoso");
MySnackbar.MessageQueue.Enqueue("¿Deshacer?", "Deshacer", 
    action: () => { /* callback */ });
```

---

## 9. Navigation Drawer

```xml
<materialDesign:DialogHost Identifier="RootDialog">
    <materialDesign:DrawerHost IsLeftDrawerOpen="{Binding IsMenuOpen}">
        
        <!-- Menú lateral -->
        <materialDesign:DrawerHost.LeftDrawerContent>
            <StackPanel Width="280">
                <materialDesign:ColorZone Mode="PrimaryMid" 
                                          Padding="16" 
                                          materialDesign:ElevationAssist.Elevation="Dp4">
                    <TextBlock Text="Mi Aplicación" 
                               Style="{StaticResource MaterialDesignHeadline6TextBlock}"/>
                </materialDesign:ColorZone>
                
                <ListBox>
                    <ListBoxItem Style="{StaticResource MaterialDesignNavigationListBoxItem}">
                        <StackPanel Orientation="Horizontal">
                            <materialDesign:PackIcon Kind="Home" Margin="0,0,16,0"/>
                            <TextBlock Text="Inicio"/>
                        </StackPanel>
                    </ListBoxItem>
                    
                    <ListBoxItem Style="{StaticResource MaterialDesignNavigationListBoxItem}">
                        <StackPanel Orientation="Horizontal">
                            <materialDesign:PackIcon Kind="Cog" Margin="0,0,16,0"/>
                            <TextBlock Text="Configuración"/>
                        </StackPanel>
                    </ListBoxItem>
                </ListBox>
            </StackPanel>
        </materialDesign:DrawerHost.LeftDrawerContent>
        
        <!-- Contenido principal -->
        <Grid>
            <Button Content="☰ Menú"
                    HorizontalAlignment="Left"
                    Margin="16"
                    Click="ToggleMenu_Click"/>
        </Grid>
    </materialDesign:DrawerHost>
</materialDesign:DialogHost>
```

---

## 10. Themes Dinámicos

### Cambiar tema en runtime
```csharp
// Cambiar entre claro/oscuro
var paletteHelper = new PaletteHelper();
var theme = paletteHelper.GetTheme();

theme.SetBaseTheme(BaseTheme.Dark); // o BaseTheme.Light
theme.SetPrimaryColor(Colors.Indigo);
theme.SetSecondaryColor(Colors.Teal);

paletteHelper.SetTheme(theme);
```

### Tema Oscuro en App.xaml
```xml
<materialDesign:BundledTheme BaseTheme="Dark" 
    PrimaryColor="Indigo" 
    SecondaryColor="Teal"/>
```

---

## 11. Tipografía Material

| Estilo | Uso | Tamaño |
|--------|-----|--------|
| H1 | Títulos principales | 96sp |
| H2 | Títulos grandes | 60sp |
| H3 | Títulos | 48sp |
| H4 | Títulos | 34sp |
| H5 | Títulos | 24sp |
| H6 | Subtítulos | 20sp |
| Subtitle1 | Subtítulos | 16sp |
| Body1 | Texto cuerpo | 16sp |
| Body2 | Texto secundario | 14sp |
| Button | Etiquetas | 14sp |
| Caption | Notas | 12sp |

---

## Recursos
- [MaterialDesignThemes GitHub](https://github.com/MaterialDesignInXAML/MaterialDesignThemes)
- [Demo App](https://github.com/MaterialDesignInXAML/MaterialDesignInXAMLToolkit/releases)
- [Icon Gallery](https://materialdesignicons.com/)
