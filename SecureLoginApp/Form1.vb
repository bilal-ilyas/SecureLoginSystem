Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Configuration
Public Class Form1
    Private Function GenerateSalt() As Byte()
        Dim salt(15) As Byte
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(salt)
        End Using
        Return salt
    End Function
    Private Function HashPassword(password As String, salt As Byte()) As Byte()
        ' PBKDF2 with 100,000 iterations
        Using pbkdf2 = New Rfc2898DeriveBytes(password, salt, 100000)
            Return pbkdf2.GetBytes(32) ' 256-bit hash
        End Using
    End Function
    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Dim username = txtUsername.Text.Trim()
        Dim password = txtPassword.Text
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Please enter both username and password.")
            Return
        End If
        Dim salt = GenerateSalt()
        Dim hash = HashPassword(password, salt)
        Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("INSERT INTO Users (Username, PasswordHash, PasswordSalt) VALUES (@u, @h, @s)", conn)
            cmd.Parameters.AddWithValue("@u", username)
            cmd.Parameters.AddWithValue("@h", hash)
            cmd.Parameters.AddWithValue("@s", salt)
            Try
                cmd.ExecuteNonQuery()
                MessageBox.Show("User registered successfully.")
            Catch ex As SqlException
                If ex.Number = 2627 Then ' Unique constraint violation - username already exists
                    MessageBox.Show("Username already exists. Please choose another.")
                Else
                    MessageBox.Show("Error: " & ex.Message)
                End If
            End Try
        End Using
    End Sub
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username = txtUsername.Text.Trim()
        Dim password = txtPassword.Text
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Please enter both username and password.")
            Return
        End If
        Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT PasswordHash, PasswordSalt FROM Users WHERE Username = @u", conn)
            cmd.Parameters.AddWithValue("@u", username)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    Dim storedHash = CType(reader("PasswordHash"), Byte())
                    Dim storedSalt = CType(reader("PasswordSalt"), Byte())
                    Dim inputHash = HashPassword(password, storedSalt)

                    If storedHash.SequenceEqual(inputHash) Then
                        MessageBox.Show("Login successful!")
                    Else
                        MessageBox.Show("Incorrect password.")
                    End If
                Else
                    MessageBox.Show("User not found.")
                End If
            End Using
        End Using
    End Sub
End Class
