''' <summary>
''' This is the main type for your game
''' </summary>
Public Class Game1
    Inherits Microsoft.Xna.Framework.Game

    Private WithEvents graphics As GraphicsDeviceManager
    Private WithEvents spriteBatch As SpriteBatch


    'GameState
    Private Enum GameStates
        Passage
        Menu
        ChoixNiveau
        Niveau1
        Niveau2
        Niveau3
        Niveau4
        Niveau5
    End Enum

    'Le Programme commence dans un passage 
    Private GameState As GameStates = GameStates.Passage

    'Pour savoir quelle touche est appuyée
    Private _KeyboardState1 As KeyboardState


    Dim Pistolet As Murs
    Dim QuelleTouche As Integer

    'Souris
    Dim Curseur As Murs

    ' Structure basique des Cases/Tiles
    Private Structure Murs
        Dim Image As Texture2D
        Dim Position As Vector2
        Dim rec As Rectangle
        Dim Contient As String
        Dim Libre As Boolean
        'Pour savoir quelle image dans les Tiles 
        Dim Code As Integer
    End Structure



    'Structure des Unités
    Private Structure Unit
        Dim Image() As Texture2D
        Dim Position As Vector2
        'La hitbox de l'unité
        Dim rec As Rectangle
        'hitbox des côtés de l'unité
        Dim recDroit As Rectangle
        Dim recGauche As Rectangle
        Dim recHaut As Rectangle
        Dim recBas As Rectangle
        Dim LibreDroit As Boolean
        Dim LibreGauche As Boolean
        Dim LibreHaut As Boolean
        Dim LibreBas As Boolean
        'Quelle image a l'unité
        Dim Qui As Integer
        'Pour que les images de l'unité ne défilent pas trop vite
        Dim TimerImage As Double
        'POUR LES UNITES ENNEMIES
        'Pour savoir si l'ennemi est entrain d'attaquer
        Dim EnAttaque As Boolean
        Dim bal1 As Murs
        Dim bal2 As Murs
        Dim EnVie As Boolean
    End Structure



    'TILES
    Dim Ligne(22) As String
    'Le nombre de Tiles différents
    Dim Tile(500) As Texture2D
    'La Variable " NbCarac " Prend 3caractères dans le tableau de Tiles  , ( Val(NbCarac)) ---> Pour une variable en Integer )    
    Dim NbCarac As String
    'Variable pour Lire le tableau de tiles , se réinitialise à chaque nouvelle ligne
    Dim Bil As Integer = 1
    'Structure Murs ====>    Tiles(i,j).Image    , Tiles(i,j).Position    , Tiles(i,j).Rec    et Tiles(i,j).Code   (qui correspond à l'image ( en integer ) contenue dans le Tile )
    'i = x et j = y
    Dim Tiles(22, 39) As Murs
    'Pour Load Les Tiles
    Dim Nom As String
    Dim QuelTile As Integer = 0


    'BALLES
    'Position d'arrivée des balles quand clique
    Dim BalValeurFinX(3) As Integer
    Dim BalValeurFinY(3) As Integer
    'Pour savoir direction d'une balle
    Dim CombienDeY(3) As Integer
    Dim CombienDeX(3) As Integer
    'En fonction de la direction , avance en X et Y
    Dim PlusCombienX(3) As Double
    Dim PlusCombienY(3) As Double
    'Prend la position du perso quand clique 
    Dim PersoBalX(3) As Integer
    Dim PersoBalY(3) As Integer
    Dim BalActive(3) As Boolean
    Dim Bal(3) As Murs
    'Pour que toutes les balles ne partent pas en même temps ==> obligation de relâcher le clique pour retirer une balle
    Dim Tire As Integer = 1
    'Si Clique gauche alors True 
    Dim CliqueActif As Boolean


    'Ennemis
    Dim Enemi(10) As Unit
    Dim BalEnemiActive(10) As Boolean
    'Si le personnage voit l'ennemi
    Dim EnemiVu(10) As Boolean

    'BALLES ENNEMIS
    'Position d'arrivée des balles (Le Perso1)
    Dim BalEnemiValeurFinX(10) As Integer
    Dim BalEnemiValeurFinY(10) As Integer
    'Pour savoir direction d'une balle Enemi
    Dim CombienDeYEnemi(10) As Integer
    Dim CombienDeXEnemi(10) As Integer
    'En fonction de la direction , avance en X et Y
    Dim PlusCombienXEnemi(10) As Double
    Dim PlusCombienYEnemi(10) As Double
    'Prend la position du perso quand clique 
    Dim EnemiBalX(10) As Integer
    Dim EnemiBalY(10) As Integer
    'False lorsque la balle est active
    Dim EnemiPeutTirer(10) As Boolean
    'Nombre d'Ennemis dans le niveau
    Dim NombreEnnemi As Integer = 1

    'Largeur et Hauteur de l'écran
    Dim ScreenWidth As Integer
    Dim ScreenHeight As Integer

    Dim PretAJouer As Boolean = False


    'Savoir quelle Map va activer le passage
    Dim ChangementMap As String


    'Rectangles du menu
    Dim BouttonPlay As Murs
    Dim BouttonAttente As Murs
    Dim BouttonExit As Murs
    Dim BouttonChoixNiveau As Murs
    Dim FondMenu As Murs
    Dim ButtonLvl(5) As Murs
    Dim QuelLvl As Integer




    Public Sub Calculfenetre()
        For i = 0 To 22
            Bil = 1
            For j = 0 To 39
                Tiles(i, j).Position.Y = i * 32

                NbCarac = Mid(Ligne(i), Bil, 3)

                Tiles(i, j).Image = Tile(Val(NbCarac))

                Tiles(i, j).Code = Val(NbCarac)

                Tiles(i, j).Position.X = j * 32

                Tiles(i, j).rec = New Rectangle(Tiles(i, j).Position.X, Tiles(i, j).Position.Y, 32, 32)

                Bil = Bil + 3
            Next
        Next
    End Sub


    Public Sub Rectanglation()



        Perso.rec = New Rectangle(Perso.Position.X, Perso.Position.Y, Perso.Image(1).Width, Perso.Image(1).Height)

        Perso.recDroit = New Rectangle(Perso.Position.X + (Perso.Image(1).Width - 6), Perso.Position.Y + 11, 4, 3)
        Perso.recGauche = New Rectangle(Perso.Position.X + 3, Perso.Position.Y + 11, 4, 3)
        Perso.recBas = New Rectangle(Perso.Position.X + 6, Perso.Position.Y + (Perso.Image(1).Height - 6), 3, 4)
        Perso.recHaut = New Rectangle(Perso.Position.X + 6, Perso.Position.Y + 3, 3, 4)

        For i = 1 To 3
            Bal(i).rec = New Rectangle(Bal(i).Position.X, Bal(i).Position.Y, Bal(i).Image.Width, Bal(i).Image.Height)
        Next
        For i = 1 To NombreEnnemi
            Enemi(i).rec = New Rectangle(Enemi(i).Position.X, Enemi(i).Position.Y, 24, 31)
        Next
        For i = 1 To NombreEnnemi
            Enemi(i).bal1.rec = New Rectangle(Enemi(i).bal1.Position.X, Enemi(i).bal1.Position.Y, Enemi(i).bal1.Image.Width, Enemi(i).bal1.Image.Height)
            Enemi(i).bal2.rec = New Rectangle(Enemi(i).bal2.Position.X, Enemi(i).bal2.Position.Y, Enemi(i).bal2.Image.Width, Enemi(i).bal2.Image.Height)
        Next


        For i = 0 To 22
            For j = 0 To 39
                If Perso.recDroit.Intersects(Tiles(i, j).rec) Then

                    If Tiles(i, j).Libre = False Then
                        Perso.LibreDroit = False
                    Else
                        Perso.LibreDroit = True
                    End If
                End If

                If Perso.recGauche.Intersects(Tiles(i, j).rec) Then

                    If Tiles(i, j).Libre = False Then
                        Perso.LibreGauche = False
                    Else
                        Perso.LibreGauche = True
                    End If
                End If

                If Perso.recBas.Intersects(Tiles(i, j).rec) Then

                    If Tiles(i, j).Libre = False Then
                        Perso.LibreBas = False
                    Else
                        Perso.LibreBas = True
                    End If
                End If

                If Perso.recHaut.Intersects(Tiles(i, j).rec) Then

                    If Tiles(i, j).Libre = False Then
                        Perso.LibreHaut = False
                    Else
                        Perso.LibreHaut = True
                    End If
                End If

            Next
        Next


        If Perso.Position.X - 20 > 1232 Then
            Perso.LibreDroit = False
        End If
        If Perso.Position.X < 0 Then
            Perso.LibreGauche = False
        End If
        If Perso.Position.Y < 0 Then
            Perso.LibreHaut = False
        End If
        If Perso.Position.Y + 20 > 700 Then
            Perso.LibreBas = False
        End If

    End Sub



    Public Sub BalActives()

        'POUR LE CURSEUR
        Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
        Dim MS As MouseState
        Curseur.Position.X = Mouse.GetState.X
        Curseur.Position.Y = Mouse.GetState.Y
        'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
        MS = Mouse.GetState

        For i = 1 To 3

            If Bal(i).Position.X < 0 Or Bal(i).Position.X > graphics.PreferredBackBufferWidth Or Bal(i).Position.Y < 0 Or Bal(i).Position.Y > graphics.PreferredBackBufferHeight Then
                BalActive(i) = False
            End If

            For h = 0 To 22
                For j = 0 To 39
                    If Bal(i).rec.Intersects(Tiles(h, j).rec) And Tiles(h, j).Libre = False Then
                        BalActive(i) = False
                    End If

                Next
            Next


            If BalActive(i) = False Then
                        Bal(i).Position.X = Pistolet.Position.X
                        Bal(i).Position.Y = Pistolet.Position.Y
                        BalValeurFinX(i) = Mouse.GetState.X
                        BalValeurFinY(i) = Mouse.GetState.Y
                        PersoBalX(i) = Perso.Position.X
                        PersoBalY(i) = Perso.Position.Y

                        If BalValeurFinX(i) > Bal(i).Position.X Then
                            CombienDeX(i) = BalValeurFinX(i) - Bal(i).Position.X
                        ElseIf Bal(i).Position.X > BalValeurFinX(i) Then
                            CombienDeX(i) = Bal(i).Position.X - BalValeurFinX(i)
                        End If
                        If BalValeurFinY(i) > Bal(i).Position.Y Then
                            CombienDeY(i) = BalValeurFinY(i) - Bal(i).Position.Y
                        ElseIf Perso.Position.Y > BalValeurFinY(i) Then
                            CombienDeY(i) = Bal(i).Position.Y - BalValeurFinY(i)
                        End If

                        If CombienDeY(i) > CombienDeX(i) Then
                            PlusCombienX(i) = CombienDeX(i) / CombienDeY(i)
                            PlusCombienY(i) = 1
                        ElseIf CombienDeX(i) > CombienDeY(i) Then
                            PlusCombienX(i) = 1
                            PlusCombienY(i) = CombienDeY(i) / CombienDeX(i)
                        End If
                    End If


                    If BalActive(i) = True Then

                        If BalValeurFinX(i) >= PersoBalX(i) Then
                            Bal(i).Position.X = Bal(i).Position.X + 1 * PlusCombienX(i) * 8
                        ElseIf BalValeurFinX(i) < PersoBalX(i) Then
                            Bal(i).Position.X = Bal(i).Position.X - 1 * PlusCombienX(i) * 8
                        End If
                        If BalValeurFinY(i) >= PersoBalY(i) Then
                            Bal(i).Position.Y = Bal(i).Position.Y + 1 * PlusCombienY(i) * 8
                        ElseIf BalValeurFinY(i) < PersoBalY(i) Then
                            Bal(i).Position.Y = Bal(i).Position.Y - 1 * PlusCombienY(i) * 8
                        End If
                    End If
                Next

                If MS.LeftButton Then
            If Curseur.Position.X > Perso.Position.X And Curseur.Position.Y > Perso.Position.Y Then
                If CliqueActif = False Then
                    Perso.Qui = 1
                    Pistolet.Position.X = Perso.Position.X + 15
                    Pistolet.Position.Y = Perso.Position.Y + 17
                    Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")
                End If
            End If
            If Curseur.Position.X < Perso.Position.X And Curseur.Position.Y < Perso.Position.Y Then
                If CliqueActif = False Then
                    Perso.Qui = 5
                    Pistolet.Position.X = Perso.Position.X
                    Pistolet.Position.Y = Perso.Position.Y + 18
                    Pistolet.Image = Content.Load(Of Texture2D)("pistolet1G")
                End If
            End If
            If Curseur.Position.X > Perso.Position.X And Curseur.Position.Y < Perso.Position.Y Then
                If CliqueActif = False Then
                    Perso.Qui = 9
                    Pistolet.Position.X = Perso.Position.X + 17
                    Pistolet.Position.Y = Perso.Position.Y + 8
                    Pistolet.Image = Content.Load(Of Texture2D)("pistolet1H")
                End If
            End If
            If Curseur.Position.X < Perso.Position.X And Curseur.Position.Y > Perso.Position.Y Then
                If CliqueActif = False Then
                    Perso.Qui = 13
                    Pistolet.Position.X = Perso.Position.X
                    Pistolet.Position.Y = Perso.Position.Y + 20
                    Pistolet.Image = Content.Load(Of Texture2D)("pistolet1B")
                End If
            End If

            If BalActive(1) = False And Tire = 0 Then
                BalActive(1) = True
                Tire = 1
            ElseIf BalActive(1) = True And BalActive(2) = False And Tire = 0 Then
                BalActive(2) = True
                Tire = 1
            ElseIf BalActive(1) = True And BalActive(2) = True And BalActive(3) = False And Tire = 0 Then
                BalActive(3) = True
                Tire = 1
            ElseIf BalActive(1) = True And BalActive(2) = True And BalActive(3) = True And Tire = 0 Then
                Tire = 1
            End If

        End If

        If MS.LeftButton Then
            CliqueActif = True
        Else
            CliqueActif = False
            Tire = 0
        End If

        For i = 1 To NombreEnnemi
            'Algo Enemi Attaque
            If EnemiPeutTirer(i) = False And BalEnemiActive(i) = False Then
                EnemiPeutTirer(i) = True
            End If

            For j = 1 To 3
                If Bal(j).rec.Intersects(Enemi(i).rec) Then
                    BalActive(j) = False
                    Enemi(i).EnVie = False
                    Enemi(i).Position.X = 2000
                End If
            Next

            If BalEnemiActive(i) = False Then
                Enemi(i).bal1.Position.X = Enemi(i).Position.X + 5
                Enemi(i).bal1.Position.Y = Enemi(i).Position.Y + 5
                Enemi(i).bal2.Position.X = Enemi(i).Position.X + 5
                Enemi(i).bal2.Position.Y = Enemi(i).Position.Y + 5

                BalEnemiValeurFinX(i) = Perso.Position.X
                BalEnemiValeurFinY(i) = Perso.Position.Y
                EnemiBalX(i) = Enemi(i).Position.X
                EnemiBalY(i) = Enemi(i).Position.Y

                If BalEnemiValeurFinX(i) > Enemi(i).bal1.Position.X Then
                    CombienDeXEnemi(i) = BalEnemiValeurFinX(i) - Enemi(i).bal1.Position.X
                ElseIf Enemi(i).bal1.Position.X > BalEnemiValeurFinX(i) Then
                    CombienDeXEnemi(i) = Enemi(i).bal1.Position.X - BalEnemiValeurFinX(i)
                End If
                If BalEnemiValeurFinY(i) > Enemi(i).bal1.Position.Y Then
                    CombienDeYEnemi(i) = BalEnemiValeurFinY(i) - Enemi(i).bal1.Position.Y
                ElseIf Enemi(i).bal1.Position.Y > BalEnemiValeurFinY(i) Then
                    CombienDeYEnemi(i) = Enemi(i).bal1.Position.Y - BalEnemiValeurFinY(i)
                End If

                If CombienDeYEnemi(i) > CombienDeXEnemi(i) Then
                    PlusCombienXEnemi(i) = CombienDeXEnemi(i) / CombienDeYEnemi(i)
                    PlusCombienYEnemi(i) = 1
                ElseIf CombienDeXEnemi(i) > CombienDeYEnemi(i) Then
                    PlusCombienXEnemi(i) = 1
                    PlusCombienYEnemi(i) = CombienDeYEnemi(i) / CombienDeXEnemi(i)
                End If

            End If

            If BalEnemiActive(i) = True Then
                If BalEnemiValeurFinX(i) >= EnemiBalX(i) Then
                    Enemi(i).bal1.Position.X = Enemi(i).bal1.Position.X + 1 * PlusCombienXEnemi(i) * 8
                ElseIf BalEnemiValeurFinX(i) < EnemiBalX(i) Then
                    Enemi(i).bal1.Position.X = Enemi(i).bal1.Position.X - 1 * PlusCombienXEnemi(i) * 8
                End If
                If BalEnemiValeurFinY(i) >= EnemiBalY(i) Then
                    Enemi(i).bal1.Position.Y = Enemi(i).bal1.Position.Y + 1 * PlusCombienYEnemi(i) * 8
                ElseIf BalEnemiValeurFinY(i) < EnemiBalY(i) Then
                    Enemi(i).bal1.Position.Y = Enemi(i).bal1.Position.Y - 1 * PlusCombienYEnemi(i) * 8
                End If

            End If


            If Enemi(i).bal1.Position.X < 0 Or Enemi(i).bal1.Position.X > graphics.PreferredBackBufferWidth Or Enemi(i).bal1.Position.Y < 0 Or Enemi(i).bal1.Position.Y > graphics.PreferredBackBufferHeight Then
                BalEnemiActive(i) = False
            End If

            For k = 0 To 22
                For l = 0 To 39
                    If Enemi(i).bal1.rec.Intersects(Tiles(k, l).rec) And Tiles(k, l).Libre = False Then
                        BalEnemiActive(i) = False
                    End If
                Next
            Next
            If Enemi(i).bal1.rec.Intersects(Perso.rec) Then
                If GameState = GameStates.Niveau1 Then
                    ChangementMap = "Niveau1"
                    GameState = GameStates.Passage
                End If
                If GameState = GameStates.Niveau2 Then
                    ChangementMap = "Niveau2"
                    GameState = GameStates.Passage
                End If
                If GameState = GameStates.Niveau3 Then
                    ChangementMap = "Niveau3"
                    GameState = GameStates.Passage
                End If
                If GameState = GameStates.Niveau4 Then
                    ChangementMap = "Niveau4"
                    GameState = GameStates.Passage
                End If
                If GameState = GameStates.Niveau5 Then
                    ChangementMap = "Niveau5"
                    GameState = GameStates.Passage
                End If

            End If

            If BalEnemiActive(i) = True Then
                EnemiPeutTirer(i) = False
            End If
            If EnemiPeutTirer(i) = True And Enemi(i).EnVie = True Then
                BalEnemiActive(i) = True
            End If
        Next



    End Sub


    Public Sub MouvementPerso()
        ' MOUVEMENT DU PERSO 1

        'Utilisé pour le mouvement des Perso
        _KeyboardState1 = Keyboard.GetState()

        ' VERS LE HAUT
        If (_KeyboardState1.IsKeyDown(Keys.Up)) Then

            QuelleTouche = 3
            If Perso.LibreHaut = True Then
                Perso.Position.Y = Perso.Position.Y - 3
            End If

            If Perso.Qui < 9 Then
                Perso.Qui = 9
            Else
                Perso.TimerImage = Perso.TimerImage + 0.2
                If Perso.TimerImage > 1 Then
                    Perso.Qui = Perso.Qui + 1
                    Perso.TimerImage = 0
                End If
            End If
            If Perso.Qui > 12 Then
                Perso.Qui = 9
            End If


            Pistolet.Position.X = Perso.Position.X + 17
            Pistolet.Position.Y = Perso.Position.Y + 8
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1H")

            'VERS LE BAS
        ElseIf (_KeyboardState1.IsKeyDown(Keys.Down)) Then


            QuelleTouche = 4
            If Perso.LibreBas = True Then
                Perso.Position.Y = Perso.Position.Y + 3
            End If

            Perso.TimerImage = Perso.TimerImage + 0.2
            If Perso.TimerImage > 1 Then
                Perso.Qui = Perso.Qui + 1
                Perso.TimerImage = 0
            End If

            If Perso.Qui < 13 Then
                Perso.Qui = 13
            End If
            If Perso.Qui > 16 Then
                Perso.Qui = 13
            End If


            Pistolet.Position.X = Perso.Position.X
            Pistolet.Position.Y = Perso.Position.Y + 20
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1B")

            'VERS LA DROITE
        ElseIf (_KeyboardState1.IsKeyDown(Keys.Right)) Then


            QuelleTouche = 1
            If Perso.LibreDroit = True Then
                Perso.Position.X = Perso.Position.X + 3
            End If


            Perso.TimerImage = Perso.TimerImage + 0.2
            If Perso.TimerImage > 1 Then
                Perso.Qui = Perso.Qui + 1
                Perso.TimerImage = 0
            End If

            If Perso.Qui > 4 Then
                Perso.Qui = 1
            End If



            Pistolet.Position.X = Perso.Position.X + 15
            Pistolet.Position.Y = Perso.Position.Y + 17
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")

            'VERS LA GAUCHE
        ElseIf (_KeyboardState1.IsKeyDown(Keys.Left)) Then


            QuelleTouche = 2
            If Perso.LibreGauche = True Then
                Perso.Position.X = Perso.Position.X - 3
            End If

            If Perso.Qui < 5 Then
                Perso.Qui = 5
            Else
                Perso.TimerImage = Perso.TimerImage + 0.2
                If Perso.TimerImage > 1 Then
                    Perso.Qui = Perso.Qui + 1
                    Perso.TimerImage = 0
                End If
            End If
            If Perso.Qui > 8 Then
                Perso.Qui = 5
            End If



            Pistolet.Position.X = Perso.Position.X
            Pistolet.Position.Y = Perso.Position.Y + 18
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1G")

        End If
    End Sub

    Dim Perso As Unit




    Public Sub New()
        graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"
    End Sub
    ''' <summary>
    ''' Allows the game to perform any initialization it needs to before starting to run.
    ''' This is where it can query for any required services and load any non-graphic
    ''' related content.  Calling MyBase.Initialize will enumerate through any components
    ''' and initialize them as well.
    ''' </summary>
    ''' 

    Protected Overrides Sub Initialize()
        ' TODO: Add your initialization logic here
        MyBase.Initialize()

        'A CHANGER PLUS TARD 
        For i = 1 To 10
            Enemi(i).Qui = 1
        Next

        BouttonPlay.Position.X = 200
        BouttonPlay.Position.Y = 200
        BouttonPlay.rec = New Rectangle(BouttonPlay.Position.X, BouttonPlay.Position.Y, BouttonPlay.Image.Width, BouttonPlay.Image.Height)

        BouttonExit.Position.X = 1000
        BouttonExit.Position.Y = 600
        BouttonExit.rec = New Rectangle(BouttonExit.Position.X, BouttonExit.Position.Y, BouttonExit.Image.Width, BouttonExit.Image.Height)

        BouttonChoixNiveau.Position.X = 200
        BouttonChoixNiveau.Position.Y = 280
        BouttonChoixNiveau.rec = New Rectangle(BouttonChoixNiveau.Position.X, BouttonChoixNiveau.Position.Y, BouttonChoixNiveau.Image.Width, BouttonChoixNiveau.Image.Height)


        BouttonAttente.Position.X = 0
        BouttonAttente.Position.Y = 633
        BouttonAttente.rec = New Rectangle(BouttonAttente.Position.X, BouttonAttente.Position.Y, BouttonAttente.Image.Width, BouttonAttente.Image.Height)


        For i = 1 To 5
            ButtonLvl(i).Position.X = 0
            ButtonLvl(i).Position.Y = 633
            ButtonLvl(i).rec = New Rectangle(ButtonLvl(i).Position.X, ButtonLvl(i).Position.Y, ButtonLvl(i).Image.Width, ButtonLvl(i).Image.Height)
        Next

        FondMenu.Position.X = 0
        FondMenu.Position.Y = 0


        ScreenWidth = 1280
        ScreenHeight = 720



      




        'Recadrer correctement le jeu en fonction de la taille de l'ecran


        ' graphics.PreferredBackBufferHeight = My.Computer.Screen.Bounds.Height
        ' graphics.PreferredBackBufferWidth = My.Computer.Screen.Bounds.Width
        'graphics.ToggleFullScreen()


        graphics.PreferredBackBufferHeight = 720
        graphics.PreferredBackBufferWidth = 1280

        '  graphics.IsFullScreen = True

        'PLEIN ECRAN PLEIN ECRAN PLEIN ECRAN PLEIN ECRAN PLEIN ECRAN PLEIN ECRAN
        'graphics.ToggleFullScreen()

        graphics.ApplyChanges()

        ChangementMap = "Menu"

    End Sub

    ''' <summary>
    ''' LoadContent will be called once per game and is the place to load
    ''' all of your content.
    ''' </summary>
    ''' 




    Protected Overrides Sub LoadContent()
        ' Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = New SpriteBatch(GraphicsDevice)

        ' TODO: use Me.Content to load your game content here



        BouttonPlay.Image = Content.Load(Of Texture2D)("ButtonPlay")
        BouttonExit.Image = Content.Load(Of Texture2D)("BouttonExit")
        BouttonChoixNiveau.Image = Content.Load(Of Texture2D)("BouttonChoixNiveau")
        BouttonAttente.Image = Content.Load(Of Texture2D)("ButtonAttente")
        FondMenu.Image = Content.Load(Of Texture2D)("FondMenu")

        For i = 1 To 5
            ButtonLvl(i).Image = Content.Load(Of Texture2D)("Lvl" & i & "")
        Next

        ReDim Perso.Image(16)
        For i = 1 To 10
            ReDim Enemi(i).Image(16)
        Next

        'Le Perso 
        Perso.Image(1) = Content.Load(Of Texture2D)("PersoD1")
        Perso.Image(2) = Content.Load(Of Texture2D)("PersoD2")
        Perso.Image(3) = Content.Load(Of Texture2D)("PersoD3")
        Perso.Image(4) = Content.Load(Of Texture2D)("PersoD4")
        Perso.Image(5) = Content.Load(Of Texture2D)("PersoG1")
        Perso.Image(6) = Content.Load(Of Texture2D)("PersoG2")
        Perso.Image(7) = Content.Load(Of Texture2D)("PersoG3")
        Perso.Image(8) = Content.Load(Of Texture2D)("PersoG4")
        Perso.Image(9) = Content.Load(Of Texture2D)("PersoH1")
        Perso.Image(10) = Content.Load(Of Texture2D)("PersoH2")
        Perso.Image(11) = Content.Load(Of Texture2D)("PersoH3")
        Perso.Image(12) = Content.Load(Of Texture2D)("PersoH4")
        Perso.Image(13) = Content.Load(Of Texture2D)("PersoB1")
        Perso.Image(14) = Content.Load(Of Texture2D)("PersoB2")
        Perso.Image(15) = Content.Load(Of Texture2D)("PersoB3")
        Perso.Image(16) = Content.Load(Of Texture2D)("PersoB4")


        'Image de la souris
        Curseur.Image = Content.Load(Of Texture2D)("Curseur")

        'Image des balles du Perso1
        For i = 1 To 3
            Bal(i).Image = Content.Load(Of Texture2D)("Balle")
        Next

        Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")



        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'RAJOUTER IMAGE ENNEMIS

        For i = 1 To 10
            Enemi(i).Image(1) = Content.Load(Of Texture2D)("CPersoB1")
        Next
        For i = 1 To 10
            Enemi(i).bal1.Image = Content.Load(Of Texture2D)("Balle")
            Enemi(i).bal2.Image = Content.Load(Of Texture2D)("Balle")
        Next






    End Sub






    ''' <summary>
    ''' UnloadContent will be called once per game and is the place to unload
    ''' all content.
    ''' </summary>
    ''' 
    Protected Overrides Sub UnloadContent()
        ' TODO: Unload any non ContentManager content here
    End Sub
    ''' <summary>
    ''' Allows the game to run logic such as updating the world,
    ''' checking for collisions, gathering input, and playing audio.
    ''' </summary>
    ''' <param name="gameTime">Provides a snapshot of timing values.</param>
    ''' 




    Protected Overrides Sub Update(ByVal gameTime As GameTime)
        ' Allows the game to exit
        If GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed Then
            Me.Exit()
        End If

        ' TODO: Add your update logic here

        If GameState = GameStates.Passage Then



            If ChangementMap = "Menu" Then
                GameState = GameStates.Menu
                ChangementMap = "Aucune"
            End If



            If ChangementMap = "ChoixNiveau" Then
                For i = 1 To 5
                    ButtonLvl(i).Position.X = 100
                    ButtonLvl(i).Position.Y = i * 50
                    ButtonLvl(i).rec = New Rectangle(ButtonLvl(i).Position.X, ButtonLvl(i).Position.Y, ButtonLvl(i).Image.Width, ButtonLvl(i).Image.Height)
                Next
                GameState = GameStates.ChoixNiveau
                ChangementMap = "Aucune"
            End If







            If ChangementMap = "Niveau1" Then


                'TILES

                Tile(0) = Content.Load(Of Texture2D)("Vide")
                Tile(1) = Content.Load(Of Texture2D)("Terre")
                Tile(2) = Content.Load(Of Texture2D)("TerreTrou")
                Tile(3) = Content.Load(Of Texture2D)("Caillou")

                Tile(4) = Content.Load(Of Texture2D)("Arbre1-1")
                Tile(5) = Content.Load(Of Texture2D)("Arbre1-2")
                Tile(6) = Content.Load(Of Texture2D)("Arbre1-3")
                Tile(7) = Content.Load(Of Texture2D)("Arbre1-4")


                Tile(8) = Content.Load(Of Texture2D)("3Tonneau-1")
                Tile(9) = Content.Load(Of Texture2D)("3Tonneau-2")
                Tile(10) = Content.Load(Of Texture2D)("3Tonneau-3")
                Tile(11) = Content.Load(Of Texture2D)("3Tonneau-4")


                Tile(12) = Content.Load(Of Texture2D)("Caleche-1")
                Tile(13) = Content.Load(Of Texture2D)("Caleche-2")
                Tile(14) = Content.Load(Of Texture2D)("Caleche-3")
                Tile(15) = Content.Load(Of Texture2D)("Caleche-4")
                Tile(16) = Content.Load(Of Texture2D)("Caleche-5")
                Tile(17) = Content.Load(Of Texture2D)("Caleche-6")
                Tile(18) = Content.Load(Of Texture2D)("Caleche-7")
                Tile(19) = Content.Load(Of Texture2D)("Caleche-8")
                Tile(20) = Content.Load(Of Texture2D)("Caleche-9")

                For i = 21 To 105
                    Tile(i) = Content.Load(Of Texture2D)("Terre")
                Next
                Ligne(0) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
                Ligne(1) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001004005001001001001001001001001"
                Ligne(2) = "001001001004005001001001001001001003022023024025026027028029001001001001001001001001001001006007001001001001004005001001"
                Ligne(3) = "001001001006007001001001001001001030031032033034035036037038003001001001001001001001001001001001001001001001006007001001"
                Ligne(4) = "001001001001001001001001001001001039040041042043044045046047001001001001001001001001002001001001001001001001001001001001"
                Ligne(5) = "001001001001001001001001002001001048049050051052053054055056001001001001001001001001001001001001001001001001001001001001"
                Ligne(6) = "001001001001001001001001001001001057058059060061062063064065001001001001001001001001001001001001001001001001001001001001"
                Ligne(7) = "001001001001001001001001001001001066067068069070071072073074001001001001001001001001001001001001001001001001001001001001"
                Ligne(8) = "001001001001001001001001004005001075076077078079080081008009001001001001001004005001001001001001001001001001001001001001"
                Ligne(9) = "001001001001001001001001006007001084085086087088089090010011001001001001001006007001001001001001001001001001001001001001"
                Ligne(10) = "001001001001001001001001001001001093094095096097098099100101001001001001001001001001001001001001001001001001001001001001"
                Ligne(11) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
                Ligne(12) = "001001001003001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001003001001001"
                Ligne(13) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
                Ligne(14) = "001001001001001001001001001001001001001001001001001001001001001001003001001001001002001001001001001001001001001001001001"
                Ligne(15) = "001001001001001001002001001001001001001001001001001001001001001001001001001004005001001001001001001001001001001001001002"
                Ligne(16) = "001001001001001001001001001001001003001001001001001001001001001001012013014006007001001001001001001001001001001001001001"
                Ligne(17) = "001001001001001001001001001001001001001001001001001001002001001001015016017001001001001001001001001001001001001001001001"
                Ligne(18) = "001001001001001001001001001001001001001001001001001001001001001001018019020001001001001001001001001001001001001001001001"
                Ligne(19) = "001001001001001001001001004005001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
                Ligne(20) = "001001001001001001001003006007001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
                Ligne(21) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
                Ligne(22) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"

                'TILES


                Calculfenetre()

                'METTRE LES BONS LIBRE FALSE
                For i = 0 To 22
                    For j = 0 To 39
                        If Tiles(i, j).Code >= 4 And Tiles(i, j).Code <= 20 Then
                            Tiles(i, j).Libre = False
                        Else
                            Tiles(i, j).Libre = True
                        End If
                    Next
                Next

                Perso.Position.X = 1100
                Perso.Position.Y = ScreenHeight / 2
                Perso.Qui = 13


                Pistolet.Position.X = Perso.Position.X + 15
                Pistolet.Position.Y = Perso.Position.Y + 17
                Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")
                For i = 1 To 3
                    BalActive(i) = False
                Next

                For i = 1 To 3
                    Bal(i).Position.X = Perso.Position.X + 15
                    Bal(i).Position.Y = Perso.Position.Y + 10
                Next

                Enemi(1).Image(1) = Content.Load(Of Texture2D)("CPersoD1")
                Enemi(1).Position.X = 100
                Enemi(1).Position.Y = 350
                Enemi(1).bal1.Position.X = Enemi(1).Position.X + 10
                Enemi(1).bal1.Position.Y = Enemi(1).Position.Y + 10
                Enemi(1).bal2.Position.X = Enemi(1).Position.X + 10
                Enemi(1).bal2.Position.Y = Enemi(1).Position.Y + 10
                

                NombreEnnemi = 1
                For i = 1 To NombreEnnemi
                    Enemi(i).EnVie = True
                    BalEnemiActive(i) = False
                Next
                QuelLvl = 1
                ButtonLvl(QuelLvl).Position.X = 0
                ButtonLvl(QuelLvl).Position.Y = 633
                PretAJouer = False
                GameState = GameStates.Niveau1
                ChangementMap = "Aucune"
            End If

        End If

        If ChangementMap = "Niveau2" Then


            Tile(1) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 1")
            Tile(2) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 2")
            Tile(3) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 3")
            Tile(4) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 4")
            Tile(5) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 1")
            Tile(6) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 2")
            Tile(7) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 3")
            Tile(8) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 4")
            Tile(9) = Content.Load(Of Texture2D)("parquet clair v2 varié 1")
            Tile(10) = Content.Load(Of Texture2D)("parquet clair v2 varié -2")
            Tile(11) = Content.Load(Of Texture2D)("parquet clair v2 varié 3")
            Tile(12) = Content.Load(Of Texture2D)("parquet clair v2 varié 4")
            Tile(13) = Content.Load(Of Texture2D)("parquet clair v2 1")
            Tile(14) = Content.Load(Of Texture2D)("parquet clair v2 2")
            Tile(15) = Content.Load(Of Texture2D)("parquet clair v2 3")
            Tile(16) = Content.Load(Of Texture2D)("parquet clair v2 4")
            Tile(17) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 1")
            Tile(18) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 2")
            Tile(19) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 3")
            Tile(20) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 4")
            Tile(21) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 1")
            Tile(22) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 2")
            Tile(23) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 3")
            Tile(24) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 4")
            Tile(25) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 1")
            Tile(26) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 2")
            Tile(27) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 3")
            Tile(28) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 4")
            Tile(29) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 1")
            Tile(30) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 2")
            Tile(31) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 3")
            Tile(32) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 4")
            Tile(33) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 1")
            Tile(34) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 2")
            Tile(35) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 3")
            Tile(36) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 4")
            Tile(37) = Content.Load(Of Texture2D)("parquet sombre Droit 1")
            Tile(38) = Content.Load(Of Texture2D)("parquet sombre Droit 2")
            Tile(39) = Content.Load(Of Texture2D)("parquet sombre Droit 3")
            Tile(40) = Content.Load(Of Texture2D)("parquet sombre Droit 4")
            Tile(41) = Content.Load(Of Texture2D)("parquet sombre du bas 1")
            Tile(42) = Content.Load(Of Texture2D)("parquet sombre du bas 2")
            Tile(43) = Content.Load(Of Texture2D)("parquet sombre du bas 3")
            Tile(44) = Content.Load(Of Texture2D)("parquet sombre du bas 4")
            Tile(45) = Content.Load(Of Texture2D)("parquet sombre Gauche 1")
            Tile(46) = Content.Load(Of Texture2D)("parquet sombre Gauche 2")
            Tile(47) = Content.Load(Of Texture2D)("parquet sombre Gauche 3")
            Tile(48) = Content.Load(Of Texture2D)("parquet sombre Gauche 4")
            Tile(49) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 1")
            Tile(50) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 2")
            Tile(51) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 3")
            Tile(52) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 4")
            Tile(53) = Content.Load(Of Texture2D)("parquet sombre haut 1")
            Tile(54) = Content.Load(Of Texture2D)("parquet sombre haut 2")
            Tile(55) = Content.Load(Of Texture2D)("parquet sombre haut 3")
            Tile(56) = Content.Load(Of Texture2D)("parquet sombre haut 4")
            Tile(57) = Content.Load(Of Texture2D)("3 cage pommes 1")
            Tile(58) = Content.Load(Of Texture2D)("3 cage pommes 2")
            Tile(59) = Content.Load(Of Texture2D)("3 cage pommes 3")
            Tile(60) = Content.Load(Of Texture2D)("3 cage pommes 4")
            Tile(61) = Content.Load(Of Texture2D)("3 cage 1")
            Tile(62) = Content.Load(Of Texture2D)("3 cage 2")
            Tile(63) = Content.Load(Of Texture2D)("3 cage 3")
            Tile(64) = Content.Load(Of Texture2D)("3 cage 4")
            Tile(65) = Content.Load(Of Texture2D)("CAGE CLAIR FERME")
            Tile(66) = Content.Load(Of Texture2D)("CAGE FERME SOMBRE")
            Tile(67) = Content.Load(Of Texture2D)("CAGE OUVERTE SOMBRE")
            Tile(68) = Content.Load(Of Texture2D)("étagère v2 1")
            Tile(69) = Content.Load(Of Texture2D)("étagère v2 2")
            Tile(70) = Content.Load(Of Texture2D)("étagère v2 3")
            Tile(71) = Content.Load(Of Texture2D)("étagère v2 4")
            Tile(72) = Content.Load(Of Texture2D)("étagère v2 5")
            Tile(73) = Content.Load(Of Texture2D)("étagère v2 6")
            Tile(80) = Content.Load(Of Texture2D)("Tapis Droite")
            Tile(81) = Content.Load(Of Texture2D)("Tapis Gauche")
            Tile(82) = Content.Load(Of Texture2D)("cage pomme 1")
            Tile(83) = Content.Load(Of Texture2D)("cage pomme 2")
            Tile(84) = Content.Load(Of Texture2D)("cage pomme 3")
            Tile(85) = Content.Load(Of Texture2D)("cage pomme 4")
            Tile(86) = Content.Load(Of Texture2D)("Vide")


            Ligne(0) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(1) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(2) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(3) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(4) = "086086086086086086086086086033034068069070054068069070054053054053054053054068069070050065065086086086086086086086086086"
            Ligne(5) = "086086086086086086086086086035036071072073056071072073056055056055056055056071072073052031032086086086086086086086086086"
            Ligne(6) = "086086086086086086086086086045046013014013014013014013014013014013014013014013014001002037038086086086086086086086086086"
            Ligne(7) = "086086086086086086086086086047048015016015016061062015016015016015016005006015016003004039040086086086086086086086086086"
            Ligne(8) = "086086086086086086086086086045046061062013065063064013014013014013014007008013014005002037038086086086086086086086086086"
            Ligne(9) = "086086086086086086086086086047048063064065016015016015016015016015016015016015016003004039040086086086086086086086086086"
            Ligne(10) = "086086086086086086086086086045046013014013014013006013014009010013014061062066014001002037038086086086086086086086086086"
            Ligne(11) = "086086086086086086086086086047048005006015016015016015016011012015016063064015016003004039040086086086086086086086086086"
            Ligne(12) = "086086086086086086086086086045046007008013014013014013014013014013014013014057058009002037038086086086086086086086086086"
            Ligne(13) = "086086086086086086086086086047048015066015016015016015016015016015016015016059060011004039040086086086086086086086086086"
            Ligne(14) = "086086086086086086086086086045046067066013014009010013014013014013014013014082083001002037038086086086086086086086086086"
            Ligne(15) = "086086086086086086086086086047048015016015016011012015016015016015016015016084085003004039040086086086086086086086086086"
            Ligne(16) = "086086086086086086086086086021022041042041042041042041042041042041042041042041042017018025026086086086086086086086086086"
            Ligne(17) = "086086086086086086086086086023024043044043044043044043044043044043044043044081080019067067066086086086086086086086086086"
            Ligne(18) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(19) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(20) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(21) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(22) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"


            'TILES

            Calculfenetre()

            'METTRE LES BONS LIBRE FALSE
            For i = 0 To 22
                For j = 0 To 39
                    If Tiles(i, j).Code >= 57 And Tiles(i, j).Code <= 73 Or Tiles(i, j).Code >= 82 And Tiles(i, j).Code <= 85 Then
                        Tiles(i, j).Libre = False
                    Else
                        Tiles(i, j).Libre = True
                    End If
                Next
            Next


            Perso.Position.X = 893
            Perso.Position.Y = 365
            Perso.Qui = 13


            Pistolet.Position.X = Perso.Position.X + 15
            Pistolet.Position.Y = Perso.Position.Y + 17
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")
            For i = 1 To 3
                BalActive(i) = False
            Next

            For i = 1 To 3
                Bal(i).Position.X = Perso.Position.X + 15
                Bal(i).Position.Y = Perso.Position.Y + 10
            Next

            Enemi(1).Image(1) = Content.Load(Of Texture2D)("CPersoD1")
            Enemi(1).Position.X = 599
            Enemi(1).Position.Y = 206
            Enemi(1).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(1).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(2).Image(1) = Content.Load(Of Texture2D)("CPersoD1")
            Enemi(2).Position.X = 529
            Enemi(2).Position.Y = 544
            Enemi(2).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(2).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal2.Position.Y = Enemi(1).Position.Y + 10


            NombreEnnemi = 2
            For i = 1 To NombreEnnemi
                Enemi(i).EnVie = True
                BalEnemiActive(i) = False
            Next
            QuelLvl = 2
            ButtonLvl(QuelLvl).Position.X = 0
            ButtonLvl(QuelLvl).Position.Y = 633
            PretAJouer = False

            GameState = GameStates.Niveau2
            ChangementMap = "Aucune"
        End If


        If ChangementMap = "Niveau3" Then

            'TILES

            Tile(0) = Content.Load(Of Texture2D)("Vide")
            Tile(1) = Content.Load(Of Texture2D)("Terre")
            Tile(2) = Content.Load(Of Texture2D)("TerreTrou")
            Tile(3) = Content.Load(Of Texture2D)("Caillou")

            Tile(4) = Content.Load(Of Texture2D)("Arbre1-1")
            Tile(5) = Content.Load(Of Texture2D)("Arbre1-2")
            Tile(6) = Content.Load(Of Texture2D)("Arbre1-3")
            Tile(7) = Content.Load(Of Texture2D)("Arbre1-4")


            Tile(8) = Content.Load(Of Texture2D)("3Tonneau-1")
            Tile(9) = Content.Load(Of Texture2D)("3Tonneau-2")
            Tile(10) = Content.Load(Of Texture2D)("3Tonneau-3")
            Tile(11) = Content.Load(Of Texture2D)("3Tonneau-4")


            Tile(12) = Content.Load(Of Texture2D)("Caleche-1")
            Tile(13) = Content.Load(Of Texture2D)("Caleche-2")
            Tile(14) = Content.Load(Of Texture2D)("Caleche-3")
            Tile(15) = Content.Load(Of Texture2D)("Caleche-4")
            Tile(16) = Content.Load(Of Texture2D)("Caleche-5")
            Tile(17) = Content.Load(Of Texture2D)("Caleche-6")
            Tile(18) = Content.Load(Of Texture2D)("Caleche-7")
            Tile(19) = Content.Load(Of Texture2D)("Caleche-8")
            Tile(20) = Content.Load(Of Texture2D)("Caleche-9")

            For i = 21 To 105
                Tile(i) = Content.Load(Of Texture2D)("Terre")
            Next
            Ligne(0) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(1) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001004005001001001001001001001001"
            Ligne(2) = "001001001004005001001001001001001003022023024025026027028029001001001001001001001001001001006007001001001001004005001001"
            Ligne(3) = "001001001006007001001001001001001030031032033034035036037038003001001001001001001001001001001001001001001001006007001001"
            Ligne(4) = "001001001001001001001001001001001039040041042043044045046047001001001001001001001001002001001001001001001001001001001001"
            Ligne(5) = "001001001001001001001001002001001048049050051052053054055056001001001001001001001001001001001001001001001001001001001001"
            Ligne(6) = "001001001001001001001001001001001057058059060061062063064065001001001001001001001001001001001001001001001001001001001001"
            Ligne(7) = "001001001001001001001001001001001066067068069070071072073074001001001001001001001001001001001001001001001001001001001001"
            Ligne(8) = "001001001001001001001001004005001075076077078079080081008009001001001001001004005001001001001001001001001001001001001001"
            Ligne(9) = "001001001001001001001001006007001084085086087088089090010011001001001001001006007001001001001001001001001001001001001001"
            Ligne(10) = "001001001001001001001001001001001093094095096097098099100101001001001001001001001001001001001001001001001001001001001001"
            Ligne(11) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(12) = "001001001003001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001003001001001"
            Ligne(13) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(14) = "001001001001001001001001001001001001001001001001001001001001001001003001001001001002001001001001001001001001001001001001"
            Ligne(15) = "001001001001001001002001001001001001001001001001001001001001001001001001001004005001001001001001001001001001001001001002"
            Ligne(16) = "001001001001001001001001001001001003001001001001001001001001001001012013014006007001001001001001001001001001001001001001"
            Ligne(17) = "001001001001001001001001001001001001001001001001001001002001001001015016017001001001001001001001001001001001001001001001"
            Ligne(18) = "001001001001001001001001001001001001001001001001001001001001001001018019020001001001001001001001001001001001001001001001"
            Ligne(19) = "001001001001001001001001004005001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(20) = "001001001001001001001003006007001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(21) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(22) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"

            'TILES


            Calculfenetre()

            'METTRE LES BONS LIBRE FALSE
            For i = 0 To 22
                For j = 0 To 39
                    If Tiles(i, j).Code >= 4 And Tiles(i, j).Code <= 20 Then
                        Tiles(i, j).Libre = False
                    Else
                        Tiles(i, j).Libre = True
                    End If
                Next
            Next

            Perso.Position.X = 840
            Perso.Position.Y = 589
            Perso.Qui = 13


            Pistolet.Position.X = Perso.Position.X + 15
            Pistolet.Position.Y = Perso.Position.Y + 17
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")
            For i = 1 To 3
                BalActive(i) = False
            Next

            For i = 1 To 3
                Bal(i).Position.X = Perso.Position.X + 15
                Bal(i).Position.Y = Perso.Position.Y + 10
            Next

            Enemi(1).Image(1) = Content.Load(Of Texture2D)("CPersoD1")
            Enemi(1).Position.X = 154
            Enemi(1).Position.Y = 683
            Enemi(1).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(1).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(2).Image(1) = Content.Load(Of Texture2D)("CPersoB1")
            Enemi(2).Position.X = 1126
            Enemi(2).Position.Y = 36
            Enemi(2).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(2).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(3).Image(1) = Content.Load(Of Texture2D)("CPersoG1")
            Enemi(3).Position.X = 1245
            Enemi(3).Position.Y = 576
            Enemi(3).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(3).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(3).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(3).bal2.Position.Y = Enemi(1).Position.Y + 10


            NombreEnnemi = 3
            For i = 1 To NombreEnnemi
                Enemi(i).EnVie = True
                BalEnemiActive(i) = False
            Next
            QuelLvl = 3
            ButtonLvl(QuelLvl).Position.X = 0
            ButtonLvl(QuelLvl).Position.Y = 633
            PretAJouer = False
            GameState = GameStates.Niveau3
            ChangementMap = "Aucune"

        End If

        If ChangementMap = "Niveau4" Then

            Tile(1) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 1")
            Tile(2) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 2")
            Tile(3) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 3")
            Tile(4) = Content.Load(Of Texture2D)("parquet clair v2 - tile opposé 4")
            Tile(5) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 1")
            Tile(6) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 2")
            Tile(7) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 3")
            Tile(8) = Content.Load(Of Texture2D)("parquet clair v2 varié 2 4")
            Tile(9) = Content.Load(Of Texture2D)("parquet clair v2 varié 1")
            Tile(10) = Content.Load(Of Texture2D)("parquet clair v2 varié -2")
            Tile(11) = Content.Load(Of Texture2D)("parquet clair v2 varié 3")
            Tile(12) = Content.Load(Of Texture2D)("parquet clair v2 varié 4")
            Tile(13) = Content.Load(Of Texture2D)("parquet clair v2 1")
            Tile(14) = Content.Load(Of Texture2D)("parquet clair v2 2")
            Tile(15) = Content.Load(Of Texture2D)("parquet clair v2 3")
            Tile(16) = Content.Load(Of Texture2D)("parquet clair v2 4")
            Tile(17) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 1")
            Tile(18) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 2")
            Tile(19) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 3")
            Tile(20) = Content.Load(Of Texture2D)("parquet sombre bas tile opposé 4")
            Tile(21) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 1")
            Tile(22) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 2")
            Tile(23) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 3")
            Tile(24) = Content.Load(Of Texture2D)("parquet sombre coin bas gauchev2 4")
            Tile(25) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 1")
            Tile(26) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 2")
            Tile(27) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 3")
            Tile(28) = Content.Load(Of Texture2D)("parquet sombre coin bas droitev2 4")
            Tile(29) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 1")
            Tile(30) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 2")
            Tile(31) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 3")
            Tile(32) = Content.Load(Of Texture2D)("parquet sombre coin haut droitev2 4")
            Tile(33) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 1")
            Tile(34) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 2")
            Tile(35) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 3")
            Tile(36) = Content.Load(Of Texture2D)("parquet sombre coin haut gauchev2 4")
            Tile(37) = Content.Load(Of Texture2D)("parquet sombre Droit 1")
            Tile(38) = Content.Load(Of Texture2D)("parquet sombre Droit 2")
            Tile(39) = Content.Load(Of Texture2D)("parquet sombre Droit 3")
            Tile(40) = Content.Load(Of Texture2D)("parquet sombre Droit 4")
            Tile(41) = Content.Load(Of Texture2D)("parquet sombre du bas 1")
            Tile(42) = Content.Load(Of Texture2D)("parquet sombre du bas 2")
            Tile(43) = Content.Load(Of Texture2D)("parquet sombre du bas 3")
            Tile(44) = Content.Load(Of Texture2D)("parquet sombre du bas 4")
            Tile(45) = Content.Load(Of Texture2D)("parquet sombre Gauche 1")
            Tile(46) = Content.Load(Of Texture2D)("parquet sombre Gauche 2")
            Tile(47) = Content.Load(Of Texture2D)("parquet sombre Gauche 3")
            Tile(48) = Content.Load(Of Texture2D)("parquet sombre Gauche 4")
            Tile(49) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 1")
            Tile(50) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 2")
            Tile(51) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 3")
            Tile(52) = Content.Load(Of Texture2D)("parquet sombre haut - tile opposé 4")
            Tile(53) = Content.Load(Of Texture2D)("parquet sombre haut 1")
            Tile(54) = Content.Load(Of Texture2D)("parquet sombre haut 2")
            Tile(55) = Content.Load(Of Texture2D)("parquet sombre haut 3")
            Tile(56) = Content.Load(Of Texture2D)("parquet sombre haut 4")
            Tile(57) = Content.Load(Of Texture2D)("3 cage pommes 1")
            Tile(58) = Content.Load(Of Texture2D)("3 cage pommes 2")
            Tile(59) = Content.Load(Of Texture2D)("3 cage pommes 3")
            Tile(60) = Content.Load(Of Texture2D)("3 cage pommes 4")
            Tile(61) = Content.Load(Of Texture2D)("3 cage 1")
            Tile(62) = Content.Load(Of Texture2D)("3 cage 2")
            Tile(63) = Content.Load(Of Texture2D)("3 cage 3")
            Tile(64) = Content.Load(Of Texture2D)("3 cage 4")
            Tile(65) = Content.Load(Of Texture2D)("CAGE CLAIR FERME")
            Tile(66) = Content.Load(Of Texture2D)("CAGE FERME SOMBRE")
            Tile(67) = Content.Load(Of Texture2D)("CAGE OUVERTE SOMBRE")
            Tile(68) = Content.Load(Of Texture2D)("étagère v2 1")
            Tile(69) = Content.Load(Of Texture2D)("étagère v2 2")
            Tile(70) = Content.Load(Of Texture2D)("étagère v2 3")
            Tile(71) = Content.Load(Of Texture2D)("étagère v2 4")
            Tile(72) = Content.Load(Of Texture2D)("étagère v2 5")
            Tile(73) = Content.Load(Of Texture2D)("étagère v2 6")
            Tile(80) = Content.Load(Of Texture2D)("Tapis Droite")
            Tile(81) = Content.Load(Of Texture2D)("Tapis Gauche")
            Tile(82) = Content.Load(Of Texture2D)("cage pomme 1")
            Tile(83) = Content.Load(Of Texture2D)("cage pomme 2")
            Tile(84) = Content.Load(Of Texture2D)("cage pomme 3")
            Tile(85) = Content.Load(Of Texture2D)("cage pomme 4")
            Tile(86) = Content.Load(Of Texture2D)("Vide")


            Ligne(0) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(1) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(2) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(3) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(4) = "086086086086086086086086086033034068069070054068069070054053054053054053054068069070050065065086086086086086086086086086"
            Ligne(5) = "086086086086086086086086086035036071072073056071072073056055056055056055056071072073052031032086086086086086086086086086"
            Ligne(6) = "086086086086086086086086086045046013014013014013014013014013014013014013014013014001002037038086086086086086086086086086"
            Ligne(7) = "086086086086086086086086086047048015016015016061062015016015016015016005006015016003004039040086086086086086086086086086"
            Ligne(8) = "086086086086086086086086086045046061062013065063064013014013014013014007008013014005002037038086086086086086086086086086"
            Ligne(9) = "086086086086086086086086086047048063064065016015016015016015016015016015016015016003004039040086086086086086086086086086"
            Ligne(10) = "086086086086086086086086086045046013014013014013006013014009010013014061062066014001002037038086086086086086086086086086"
            Ligne(11) = "086086086086086086086086086047048005006015016015016015016011012015016063064015016003004039040086086086086086086086086086"
            Ligne(12) = "086086086086086086086086086045046007008013014013014013014013014013014013014057058009002037038086086086086086086086086086"
            Ligne(13) = "086086086086086086086086086047048015066015016015016015016015016015016015016059060011004039040086086086086086086086086086"
            Ligne(14) = "086086086086086086086086086045046067066013014009010013014013014013014013014082083001002037038086086086086086086086086086"
            Ligne(15) = "086086086086086086086086086047048015016015016011012015016015016015016015016084085003004039040086086086086086086086086086"
            Ligne(16) = "086086086086086086086086086021022041042041042041042041042041042041042041042041042017018025026086086086086086086086086086"
            Ligne(17) = "086086086086086086086086086023024043044043044043044043044043044043044043044081080019067067066086086086086086086086086086"
            Ligne(18) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(19) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(20) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(21) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"
            Ligne(22) = "086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086086"


            'TILES

            Calculfenetre()

            'METTRE LES BONS LIBRE FALSE
            For i = 0 To 22
                For j = 0 To 39
                    If Tiles(i, j).Code >= 57 And Tiles(i, j).Code <= 73 Or Tiles(i, j).Code >= 82 And Tiles(i, j).Code <= 85 Then
                        Tiles(i, j).Libre = False
                    Else
                        Tiles(i, j).Libre = True
                    End If
                Next
            Next


            Perso.Position.X = 381
            Perso.Position.Y = 218
            Perso.Qui = 13


            Pistolet.Position.X = Perso.Position.X + 15
            Pistolet.Position.Y = Perso.Position.Y + 17
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")
            For i = 1 To 3
                BalActive(i) = False
            Next

            For i = 1 To 3
                Bal(i).Position.X = Perso.Position.X + 15
                Bal(i).Position.Y = Perso.Position.Y + 10
            Next

            Enemi(1).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(1).Position.X = 317
            Enemi(1).Position.Y = 507
            Enemi(1).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(1).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(2).Image(1) = Content.Load(Of Texture2D)("CPersoG1")
            Enemi(2).Position.X = 905
            Enemi(2).Position.Y = 302
            Enemi(2).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(2).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(3).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(3).Position.X = 610
            Enemi(3).Position.Y = 495
            Enemi(3).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(3).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(3).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(3).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(4).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(4).Position.X = 469
            Enemi(4).Position.Y = 380
            Enemi(4).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(4).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(4).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(4).bal2.Position.Y = Enemi(1).Position.Y + 10


            NombreEnnemi = 4
            For i = 1 To NombreEnnemi
                Enemi(i).EnVie = True
                BalEnemiActive(i) = False
            Next
            QuelLvl = 4
            ButtonLvl(QuelLvl).Position.X = 0
            ButtonLvl(QuelLvl).Position.Y = 633
            PretAJouer = False

            GameState = GameStates.Niveau4
            ChangementMap = "Aucune"

        End If


        If ChangementMap = "Niveau5" Then

            'TILES

            Tile(0) = Content.Load(Of Texture2D)("Vide")
            Tile(1) = Content.Load(Of Texture2D)("Terre")
            Tile(2) = Content.Load(Of Texture2D)("TerreTrou")
            Tile(3) = Content.Load(Of Texture2D)("Caillou")

            Tile(4) = Content.Load(Of Texture2D)("Arbre1-1")
            Tile(5) = Content.Load(Of Texture2D)("Arbre1-2")
            Tile(6) = Content.Load(Of Texture2D)("Arbre1-3")
            Tile(7) = Content.Load(Of Texture2D)("Arbre1-4")


            Tile(8) = Content.Load(Of Texture2D)("3Tonneau-1")
            Tile(9) = Content.Load(Of Texture2D)("3Tonneau-2")
            Tile(10) = Content.Load(Of Texture2D)("3Tonneau-3")
            Tile(11) = Content.Load(Of Texture2D)("3Tonneau-4")


            Tile(12) = Content.Load(Of Texture2D)("Caleche-1")
            Tile(13) = Content.Load(Of Texture2D)("Caleche-2")
            Tile(14) = Content.Load(Of Texture2D)("Caleche-3")
            Tile(15) = Content.Load(Of Texture2D)("Caleche-4")
            Tile(16) = Content.Load(Of Texture2D)("Caleche-5")
            Tile(17) = Content.Load(Of Texture2D)("Caleche-6")
            Tile(18) = Content.Load(Of Texture2D)("Caleche-7")
            Tile(19) = Content.Load(Of Texture2D)("Caleche-8")
            Tile(20) = Content.Load(Of Texture2D)("Caleche-9")

            For i = 21 To 105
                Tile(i) = Content.Load(Of Texture2D)("Terre")
            Next
            Ligne(0) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(1) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001004005001001001001001001001001"
            Ligne(2) = "001001001004005001001001001001001003022023024025026027028029001001001001001001001001001001006007001001001001004005001001"
            Ligne(3) = "001001001006007001001001001001001030031032033034035036037038003001001001001001001001001001001001001001001001006007001001"
            Ligne(4) = "001001001001001001001001001001001039040041042043044045046047001001001001001001001001002001001001001001001001001001001001"
            Ligne(5) = "001001001001001001001001002001001048049050051052053054055056001001001001001001001001001001001001001001001001001001001001"
            Ligne(6) = "001001001001001001001001001001001057058059060061062063064065001001001001001001001001001001001001001001001001001001001001"
            Ligne(7) = "001001001001001001001001001001001066067068069070071072073074001001001001001001001001001001001001001001001001001001001001"
            Ligne(8) = "001001001001001001001001004005001075076077078079080081008009001001001001001004005001001001001001001001001001001001001001"
            Ligne(9) = "001001001001001001001001006007001084085086087088089090010011001001001001001006007001001001001001001001001001001001001001"
            Ligne(10) = "001001001001001001001001001001001093094095096097098099100101001001001001001001001001001001001001001001001001001001001001"
            Ligne(11) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(12) = "001001001003001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001003001001001"
            Ligne(13) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(14) = "001001001001001001001001001001001001001001001001001001001001001001003001001001001002001001001001001001001001001001001001"
            Ligne(15) = "001001001001001001002001001001001001001001001001001001001001001001001001001004005001001001001001001001001001001001001002"
            Ligne(16) = "001001001001001001001001001001001003001001001001001001001001001001012013014006007001001001001001001001001001001001001001"
            Ligne(17) = "001001001001001001001001001001001001001001001001001001002001001001015016017001001001001001001001001001001001001001001001"
            Ligne(18) = "001001001001001001001001001001001001001001001001001001001001001001018019020001001001001001001001001001001001001001001001"
            Ligne(19) = "001001001001001001001001004005001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(20) = "001001001001001001001003006007001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(21) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"
            Ligne(22) = "001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001001"

            'TILES


            Calculfenetre()

            'METTRE LES BONS LIBRE FALSE
            For i = 0 To 22
                For j = 0 To 39
                    If Tiles(i, j).Code >= 4 And Tiles(i, j).Code <= 20 Then
                        Tiles(i, j).Libre = False
                    Else
                        Tiles(i, j).Libre = True
                    End If
                Next
            Next

            Perso.Position.X = 610
            Perso.Position.Y = 95
            Perso.Qui = 13


            Pistolet.Position.X = Perso.Position.X + 15
            Pistolet.Position.Y = Perso.Position.Y + 17
            Pistolet.Image = Content.Load(Of Texture2D)("pistolet1D")
            For i = 1 To 3
                BalActive(i) = False
            Next

            For i = 1 To 3
                Bal(i).Position.X = Perso.Position.X + 15
                Bal(i).Position.Y = Perso.Position.Y + 10
            Next

            Enemi(1).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(1).Position.X = 60
            Enemi(1).Position.Y = 289
            Enemi(1).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(1).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(1).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(2).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(2).Position.X = 335
            Enemi(2).Position.Y = 464
            Enemi(2).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(2).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(2).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(3).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(3).Position.X = 740
            Enemi(3).Position.Y = 420
            Enemi(3).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(3).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(3).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(3).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(4).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(4).Position.X = 1005
            Enemi(4).Position.Y = 352
            Enemi(4).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(4).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(4).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(4).bal2.Position.Y = Enemi(1).Position.Y + 10

            Enemi(5).Image(1) = Content.Load(Of Texture2D)("CPersoH1")
            Enemi(5).Position.X = 1244
            Enemi(5).Position.Y = 255
            Enemi(5).bal1.Position.X = Enemi(1).Position.X + 10
            Enemi(5).bal1.Position.Y = Enemi(1).Position.Y + 10
            Enemi(5).bal2.Position.X = Enemi(1).Position.X + 10
            Enemi(5).bal2.Position.Y = Enemi(1).Position.Y + 10


            NombreEnnemi = 5
            For i = 1 To NombreEnnemi
                Enemi(i).EnVie = True
                BalEnemiActive(i) = False
            Next
            QuelLvl = 5
            ButtonLvl(QuelLvl).Position.X = 0
            ButtonLvl(QuelLvl).Position.Y = 633
            PretAJouer = False
            GameState = GameStates.Niveau5
            ChangementMap = "Aucune"

        End If


        'PASSAGES
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'GAMEPLAY
     

        If GameState = GameStates.Menu Then
            'POUR LE CURSEUR
            Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
            Dim MS As MouseState
            Curseur.Position.X = Mouse.GetState.X
            Curseur.Position.Y = Mouse.GetState.Y
            'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
            MS = Mouse.GetState


            If Curseur.rec.Intersects(BouttonPlay.rec) Then
                If MS.LeftButton Then
                    ChangementMap = "Niveau1"
                    GameState = GameStates.Passage
                End If
            End If
            If Curseur.rec.Intersects(BouttonChoixNiveau.rec) Then
                If MS.LeftButton Then
                    ChangementMap = "ChoixNiveau"
                    GameState = GameStates.Passage
                End If
            End If
            If Curseur.rec.Intersects(BouttonExit.rec) Then
                If MS.LeftButton Then
                    End
                End If
            End If

        End If



        If GameState = GameStates.ChoixNiveau Then
            'POUR LE CURSEUR
            Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
            Dim MS As MouseState
            Curseur.Position.X = Mouse.GetState.X
            Curseur.Position.Y = Mouse.GetState.Y
            'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
            MS = Mouse.GetState

            For i = 1 To 5
                If Curseur.rec.Intersects(ButtonLvl(i).rec) Then
                    If MS.LeftButton Then
                        ChangementMap = "Niveau" & i & ""
                        GameState = GameStates.Passage
                    End If
                End If
            Next


        End If




        If GameState = GameStates.Niveau1 Then
            If PretAJouer = False Then

                Pistolet.Position.X = Perso.Position.X
                Pistolet.Position.Y = Perso.Position.Y + 20
                Pistolet.Image = Content.Load(Of Texture2D)("pistolet1B")
                'POUR LE CURSEUR
                Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
                Dim MS As MouseState
                Curseur.Position.X = Mouse.GetState.X
                Curseur.Position.Y = Mouse.GetState.Y
                'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
                MS = Mouse.GetState
                _KeyboardState1 = Keyboard.GetState()

                If (_KeyboardState1.IsKeyDown(Keys.Space)) Or (_KeyboardState1.IsKeyDown(Keys.Enter)) Then
                    PretAJouer = True
                End If
               
                
            Else
                MouvementPerso()
                Rectanglation()
                BalActives()

                If (_KeyboardState1.IsKeyDown(Keys.Escape)) Then
                    graphics.IsFullScreen = False
                    graphics.ApplyChanges()
                End If
                
                If Enemi(1).EnVie = False Then
                    ChangementMap = "Niveau2"
                    GameState = GameStates.Passage
                End If
               
            End If



        End If


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If GameState = GameStates.Niveau2 Then
            If PretAJouer = False Then

                Pistolet.Position.X = Perso.Position.X
                Pistolet.Position.Y = Perso.Position.Y + 20
                Pistolet.Image = Content.Load(Of Texture2D)("pistolet1B")
                'POUR LE CURSEUR
                Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
                Dim MS As MouseState
                Curseur.Position.X = Mouse.GetState.X
                Curseur.Position.Y = Mouse.GetState.Y
                'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
                MS = Mouse.GetState
                _KeyboardState1 = Keyboard.GetState()

                If (_KeyboardState1.IsKeyDown(Keys.Space)) Or (_KeyboardState1.IsKeyDown(Keys.Enter)) Then
                    PretAJouer = True
                End If


            Else
                MouvementPerso()
                Rectanglation()
                BalActives()

              

                If Perso.Position.X - 20 > 946 Then
                    Perso.LibreDroit = False
                End If
                If Perso.Position.X < 290 Then
                    Perso.LibreGauche = False
                End If
                If Perso.Position.Y < 112 Then
                    Perso.LibreHaut = False
                End If
                If Perso.Position.Y + 20 > 562 Then
                    Perso.LibreBas = False
                End If


                If (_KeyboardState1.IsKeyDown(Keys.Escape)) Then
                    graphics.IsFullScreen = False
                    graphics.ApplyChanges()
                End If

                If Enemi(1).EnVie = False And Enemi(2).EnVie = False Then
                    ChangementMap = "Niveau3"
                    GameState = GameStates.Passage
                End If

            End If
        End If



        If GameState = GameStates.Niveau3 Then
            If PretAJouer = False Then

                Pistolet.Position.X = Perso.Position.X
                Pistolet.Position.Y = Perso.Position.Y + 20
                Pistolet.Image = Content.Load(Of Texture2D)("pistolet1B")
                'POUR LE CURSEUR
                Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
                Dim MS As MouseState
                Curseur.Position.X = Mouse.GetState.X
                Curseur.Position.Y = Mouse.GetState.Y
                'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
                MS = Mouse.GetState
                _KeyboardState1 = Keyboard.GetState()

                If (_KeyboardState1.IsKeyDown(Keys.Space)) Or (_KeyboardState1.IsKeyDown(Keys.Enter)) Then
                    PretAJouer = True
                End If

                
            Else
                MouvementPerso()
                Rectanglation()
                BalActives()

                If (_KeyboardState1.IsKeyDown(Keys.Escape)) Then
                    graphics.IsFullScreen = False
                    graphics.ApplyChanges()
                End If

                If Enemi(1).EnVie = False And Enemi(2).EnVie = False And Enemi(3).EnVie = False Then
                    ChangementMap = "Niveau4"
                    GameState = GameStates.Passage
                End If

            End If
        End If




        If GameState = GameStates.Niveau4 Then
            If PretAJouer = False Then

                Pistolet.Position.X = Perso.Position.X
                Pistolet.Position.Y = Perso.Position.Y + 20
                Pistolet.Image = Content.Load(Of Texture2D)("pistolet1B")
                'POUR LE CURSEUR
                Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
                Dim MS As MouseState
                Curseur.Position.X = Mouse.GetState.X
                Curseur.Position.Y = Mouse.GetState.Y
                'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
                MS = Mouse.GetState
                _KeyboardState1 = Keyboard.GetState()

                If (_KeyboardState1.IsKeyDown(Keys.Space)) Or (_KeyboardState1.IsKeyDown(Keys.Enter)) Then
                    PretAJouer = True
                End If


            Else
                MouvementPerso()
                Rectanglation()
                BalActives()



                If Perso.Position.X - 20 > 946 Then
                    Perso.LibreDroit = False
                End If
                If Perso.Position.X < 290 Then
                    Perso.LibreGauche = False
                End If
                If Perso.Position.Y < 112 Then
                    Perso.LibreHaut = False
                End If
                If Perso.Position.Y + 20 > 562 Then
                    Perso.LibreBas = False
                End If


                If (_KeyboardState1.IsKeyDown(Keys.Escape)) Then
                    graphics.IsFullScreen = False
                    graphics.ApplyChanges()
                End If


                If Enemi(1).EnVie = False And Enemi(2).EnVie = False And Enemi(3).EnVie = False And Enemi(4).EnVie = False Then
                    ChangementMap = "Niveau5"
                    GameState = GameStates.Passage
                End If

            End If
        End If



        If GameState = GameStates.Niveau5 Then
            If PretAJouer = False Then

                Pistolet.Position.X = Perso.Position.X
                Pistolet.Position.Y = Perso.Position.Y + 20
                Pistolet.Image = Content.Load(Of Texture2D)("pistolet1B")
                'POUR LE CURSEUR
                Curseur.rec = New Rectangle(Mouse.GetState.X, Mouse.GetState.Y, 1, 1)
                Dim MS As MouseState
                Curseur.Position.X = Mouse.GetState.X
                Curseur.Position.Y = Mouse.GetState.Y
                'MS.TOUCHE = Si la touche " TOUCHE " est appuyée 
                MS = Mouse.GetState
                _KeyboardState1 = Keyboard.GetState()

                If (_KeyboardState1.IsKeyDown(Keys.Space)) Or (_KeyboardState1.IsKeyDown(Keys.Enter)) Then
                    PretAJouer = True
                End If

               
            Else
                MouvementPerso()
                Rectanglation()
                BalActives()

                If (_KeyboardState1.IsKeyDown(Keys.Escape)) Then
                    graphics.IsFullScreen = False
                    graphics.ApplyChanges()
                End If

                If Enemi(1).EnVie = False And Enemi(2).EnVie = False And Enemi(3).EnVie = False And Enemi(4).EnVie = False And Enemi(5).EnVie = False Then
                    ChangementMap = "Menu"
                    GameState = GameStates.Passage
                End If

            End If
        End If



            MyBase.Update(gameTime)
    End Sub



    ''' <summary>
    ''' This is called when the game should draw itself.
    ''' </summary>
    ''' <param name="gameTime">Provides a snapshot of timing values.</param>
    ''' 



    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(Color.White)

        ' TODO: Add your drawing code here

        spriteBatch.Begin()

        If GameState = GameStates.Menu Then
            spriteBatch.Draw(FondMenu.Image, FondMenu.Position, Color.White)
            spriteBatch.Draw(BouttonChoixNiveau.Image, BouttonChoixNiveau.Position, Color.White)
            spriteBatch.Draw(BouttonExit.Image, BouttonExit.Position, Color.White)
            spriteBatch.Draw(BouttonPlay.Image, BouttonPlay.Position, Color.White)
            spriteBatch.Draw(Curseur.Image, Curseur.Position, Color.White)

        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        If GameState = GameStates.ChoixNiveau Then
            spriteBatch.Draw(FondMenu.Image, FondMenu.Position, Color.White)

            For i = 1 To 5
                spriteBatch.Draw(ButtonLvl(i).Image, ButtonLvl(i).Position, Color.White)
            Next
            spriteBatch.Draw(Curseur.Image, Curseur.Position, Color.White)

        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If GameState = GameStates.Niveau1 Then
            For i = 0 To 22
                For j = 0 To 39
                    spriteBatch.Draw(Tiles(i, j).Image, Tiles(i, j).Position, Color.White)
                Next
            Next

            For i = 1 To 3
                spriteBatch.Draw(Bal(i).Image, Bal(i).Position, Color.White)
            Next
            spriteBatch.Draw(Perso.Image(Perso.Qui), Perso.Position, Color.White)
            For i = 1 To 4
                spriteBatch.Draw(Pistolet.Image, Pistolet.Position, Color.White)
            Next

            For i = 1 To NombreEnnemi
                spriteBatch.Draw(Enemi(i).bal1.Image, Enemi(i).bal1.Position, Color.White)
                spriteBatch.Draw(Enemi(i).Image(Enemi(i).Qui), Enemi(i).Position, Color.White)
            Next

            If PretAJouer = False Then
                spriteBatch.Draw(BouttonAttente.Image, BouttonAttente.Position, Color.White)
                spriteBatch.Draw(ButtonLvl(QuelLvl).Image, ButtonLvl(QuelLvl).Position, Color.White)
            End If

            spriteBatch.Draw(Curseur.Image, Curseur.Position, Color.White)
        End If


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If GameState = GameStates.Niveau2 Then
            GraphicsDevice.Clear(Color.Black)

            For i = 0 To 22
                For j = 0 To 39
                    spriteBatch.Draw(Tiles(i, j).Image, Tiles(i, j).Position, Color.White)
                Next
            Next

            For i = 1 To 3
                spriteBatch.Draw(Bal(i).Image, Bal(i).Position, Color.White)
            Next
            spriteBatch.Draw(Perso.Image(Perso.Qui), Perso.Position, Color.White)

            For i = 1 To 4
                spriteBatch.Draw(Pistolet.Image, Pistolet.Position, Color.White)
            Next
            spriteBatch.Draw(Curseur.Image, Curseur.Position, Color.White)

            For i = 1 To NombreEnnemi
                spriteBatch.Draw(Enemi(i).bal1.Image, Enemi(i).bal1.Position, Color.White)
                spriteBatch.Draw(Enemi(i).Image(Enemi(i).Qui), Enemi(i).Position, Color.White)
            Next

            If PretAJouer = False Then
                spriteBatch.Draw(BouttonAttente.Image, BouttonAttente.Position, Color.White)
                spriteBatch.Draw(ButtonLvl(QuelLvl).Image, ButtonLvl(QuelLvl).Position, Color.White)
            End If
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If GameState = GameStates.Niveau3 Then
            For i = 0 To 22
                For j = 0 To 39
                    spriteBatch.Draw(Tiles(i, j).Image, Tiles(i, j).Position, Color.White)
                Next
            Next

            For i = 1 To 3
                spriteBatch.Draw(Bal(i).Image, Bal(i).Position, Color.White)
            Next
            spriteBatch.Draw(Perso.Image(Perso.Qui), Perso.Position, Color.White)
            For i = 1 To 4
                spriteBatch.Draw(Pistolet.Image, Pistolet.Position, Color.White)
            Next

            For i = 1 To NombreEnnemi
                spriteBatch.Draw(Enemi(i).bal1.Image, Enemi(i).bal1.Position, Color.White)
                spriteBatch.Draw(Enemi(i).Image(Enemi(i).Qui), Enemi(i).Position, Color.White)
            Next

            If PretAJouer = False Then
                spriteBatch.Draw(BouttonAttente.Image, BouttonAttente.Position, Color.White)
                spriteBatch.Draw(ButtonLvl(QuelLvl).Image, ButtonLvl(QuelLvl).Position, Color.White)
            End If

            spriteBatch.Draw(Curseur.Image, Curseur.Position, Color.White)
        End If


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If GameState = GameStates.Niveau4 Then
            GraphicsDevice.Clear(Color.Black)

            For i = 0 To 22
                For j = 0 To 39
                    spriteBatch.Draw(Tiles(i, j).Image, Tiles(i, j).Position, Color.White)
                Next
            Next

            For i = 1 To 3
                spriteBatch.Draw(Bal(i).Image, Bal(i).Position, Color.White)
            Next
            spriteBatch.Draw(Perso.Image(Perso.Qui), Perso.Position, Color.White)

            For i = 1 To 4
                spriteBatch.Draw(Pistolet.Image, Pistolet.Position, Color.White)
            Next
            spriteBatch.Draw(Curseur.Image, Curseur.Position, Color.White)

            For i = 1 To NombreEnnemi
                spriteBatch.Draw(Enemi(i).bal1.Image, Enemi(i).bal1.Position, Color.White)
                spriteBatch.Draw(Enemi(i).Image(Enemi(i).Qui), Enemi(i).Position, Color.White)
            Next

            If PretAJouer = False Then
                spriteBatch.Draw(BouttonAttente.Image, BouttonAttente.Position, Color.White)
                spriteBatch.Draw(ButtonLvl(QuelLvl).Image, ButtonLvl(QuelLvl).Position, Color.White)
            End If
        End If



        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If GameState = GameStates.Niveau5 Then
            For i = 0 To 22
                For j = 0 To 39
                    spriteBatch.Draw(Tiles(i, j).Image, Tiles(i, j).Position, Color.White)
                Next
            Next

            For i = 1 To 3
                spriteBatch.Draw(Bal(i).Image, Bal(i).Position, Color.White)
            Next
            spriteBatch.Draw(Perso.Image(Perso.Qui), Perso.Position, Color.White)
            For i = 1 To 4
                spriteBatch.Draw(Pistolet.Image, Pistolet.Position, Color.White)
            Next

            For i = 1 To NombreEnnemi
                spriteBatch.Draw(Enemi(i).bal1.Image, Enemi(i).bal1.Position, Color.White)
                spriteBatch.Draw(Enemi(i).Image(Enemi(i).Qui), Enemi(i).Position, Color.White)
            Next

            If PretAJouer = False Then
                spriteBatch.Draw(BouttonAttente.Image, BouttonAttente.Position, Color.White)
                spriteBatch.Draw(ButtonLvl(QuelLvl).Image, ButtonLvl(QuelLvl).Position, Color.White)
            End If

            spriteBatch.Draw(Curseur.Image, Curseur.Position, Color.White)
        End If

        spriteBatch.End()



        MyBase.Draw(gameTime)
    End Sub

End Class
