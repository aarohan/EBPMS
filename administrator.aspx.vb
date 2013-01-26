Partial Class administrator
    Inherits System.Web.UI.Page
    Dim con As New OracleConnection(ConfigurationManager.AppSettings("ConnectionString1"))

Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'popdata()
        Dim uid As String = Session("pay_empno")
        If Not IsPostBack Then
            Panel1.Visible = False
            Panel2.Visible = False
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim empno1 As String = ""
        empno1 = TextBox1.Text
        Label2.Text = ""

        Panel1.Visible = False

        Panel2.Visible = False

        If Not IsNumeric(empno1) Then
            Label2.Text = "Please enter numeric Employee Number (EMPNO)"
            'Login1.FailureText = "Please enter numeric Employee Number"
            'Response.Write("Not numeric")
            'Exit Sub
        ElseIf IsNumeric(empno1) Then

            Dim sql5 As String = ""
            sql5 = "SELECT * FROM p_login_master where USER_NAME = '" & empno1 & "' "

            Dim cmd2 As New OracleCommand(sql5, con)
            Dim adp5 As New OracleDataAdapter(cmd2)
            Dim dt5 As New DataTable
            adp5.Fill(dt5)
            adp5.Dispose()

            Dim dtr5 As DataTableReader = dt5.CreateDataReader
            If dtr5.HasRows Then
                Panel1.Visible = True
                Panel2.Visible = True
                Label1.Visible = False
                TextBox3.Visible = False
                'TextBox2.Visible = True
                'DropDownList1.Visible = True
                'Label1.Visible = True
                'TextBox3.Visible = True
                'Button4.Visible = True
                'Button5.Visible = True
                'Dim uid As String = Session("pay_empno")

                show_grid()
            Else
                Label2.Text = "Please enter a valid Employee Number (EMPNO)"
            End If

            dtr5.Close()
            dt5.Dispose()

        End If

    End Sub
    Sub show_grid()
        Dim empno1 As String = ""
        empno1 = TextBox1.Text
        Dim sql1 As String = " "
        sql1 = "select * /*EMPNO, NAME, DESIG_ID, BANK_AC_NO, AMOUNT, P_TYPE, E_STATUS, A_STATUS, L_DATE, L_TIME, REMARKS*/ from x_e_payment /* , m_fix_employee */"
        sql1 = sql1 + " where empno = '" & empno1 & "' "
        'sql1 = sql1 + " where x_e_payment.ACCOUNT = m_fix_employee.BANK_AC_NO "
        sql1 = sql1 + " order by L_DATE desc"

        Dim cmd1 As New OracleCommand(sql1, con)
        Dim adp As New OracleDataAdapter(cmd1)
        Dim dt1 As New DataTable()
        adp.Fill(dt1)
        adp.Dispose()

        GridView1.DataSource = dt1
        GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Redirect("paycommon.aspx")
    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Session.Abandon()
        'FormsAuthentication.SignOut()
        'FormsAuthentication.RedirectToLoginPage()
        'Response.Cache.SetNoStore()
        'Response.Cache.SetNoServerCaching()

        'If Session("pay_empno") = Nothing Then 'Or Session("UserRole") <> "Admin" Then
        '    Session("ErrMsg") = "Session Expired or Access Denied please login"
        '    Response.Redirect("../Default.aspx")
        'End If

        Response.Buffer = True
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ExpiresAbsolute = DateTime.Now().AddDays(-1)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"

        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        If DropDownList1.Text = "Others" Then
            Label1.Visible = True
            TextBox3.Visible = True
            Label1.Text = "If 'Others' please specify: "
        Else
            Label1.Visible = False
            TextBox3.Visible = False
        End If
    End Sub

    Protected Sub TextBox2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        Dim amount As String = ""
        amount = TextBox2.Text
    End Sub

    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim sql4 As String = ""
        sql4 = " insert into x_e_payment values ( '" & TextBox1.Text & "', '2009A7PS001G', '" & TextBox2.Text & "',  '" & Me.DropDownList1.Text & "', 'N', 'N', sysdate, sysdate, 'Nil') "
        Dim sql6 As String = ""
        sql6 = " insert into x_e_payment values ( '" & TextBox1.Text & "', '2009A7PS001G', '" & TextBox2.Text & "',  '" & Me.TextBox3.Text & "', 'N', 'N', sysdate, sysdate, 'Nil') "
        Label3.Visible = False
        Label4.Visible = False
        If TextBox2.Text = "" Or TextBox2.Text = "0" Then
            Label4.Visible = True
            Label4.Text = "Please enter an amount"
        Else
            If DropDownList1.Text = "Others" Then
                If TextBox3.Text = "" Then
                    'Label3.Visible = True
                    'Label3.Text = "Please specify the 'Other' payment type"
                    Label1.Visible = True
                    TextBox3.Visible = True
                    Label1.Text = "If 'Others' please specify: "
                Else
                    Dim cmd1 As New OracleCommand(sql6, con)
                    Dim n1 As Integer = 0
                    con.Open()
                    n1 = cmd1.ExecuteNonQuery()
                    con.Close()
                    If n1 > 0 Then
                        Response.Write("Updated/Deleted  " + CStr(n1) + " row")
                    Else
                        Response.Write("No rows Updated/Deleted")
                    End If

                    GridView1.Dispose()
                    show_grid()
                End If
                TextBox3.Dispose()
            Else
                Dim cmd1 As New OracleCommand(sql4, con)
                Dim n1 As Integer = 0
                con.Open()
                n1 = cmd1.ExecuteNonQuery()
                con.Close()
                If n1 > 0 Then
                    Response.Write("Updated/Deleted  " + CStr(n1) + " row")
                Else
                    Response.Write("No rows Updated/Deleted")
                End If

                GridView1.Dispose()
                show_grid()
            End If
        End If
        TextBox3.Dispose()
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        Response.Redirect("paycommon.aspx")
    End Sub

    Protected Sub TextBox3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        Dim text_for_others As String = ""
        text_for_others = TextBox3.Text
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class
