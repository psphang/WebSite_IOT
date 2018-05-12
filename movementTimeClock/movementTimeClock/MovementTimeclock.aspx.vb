Imports System.IO
Imports movementTimeClock.MySQLdata
Imports movementTimeClock.sendmail
Imports movementTimeClock.Employee

Public Class MovementTimeclock
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SID As String
        
        Dim sts As Integer = 0
        Dim email_lst As String = "phang@mem.meap.com"
        Dim email_from As String = "phang@mem.meap.com"
        Dim net_sts As Integer = 0
        Dim use_ssid As String
        Dim employeeInfo As New Employee
        Dim CardNo As String
        Dim ret As String
        Dim opt As Integer = 0
        Try
            SID = sender.Request.QueryString("SID") 'Sensor ID      
            CardNo = sender.Request.QueryString("CNo")  'EM card no 
            net_sts = sender.Request.QueryString("netsts") 'Network connection status
            use_ssid = sender.Request.QueryString("SSID")   'Wifi SSID/name

            'Test Data
            net_sts = 0
            CardNo = "0008657047"
            If (net_sts > 0) Then 'Network resume send notification mail 
                Dim mailcontent As String = ""
                Dim mailsubject As String = ""

                If (net_sts = 1) Then
                    mailcontent = "The network connection start up " & Now() & ". Connected to (SSID) :" & use_ssid & ". Last success connection on : "
                    mailsubject = SID & "The network connection start up " & Now()
                ElseIf (net_sts = 2) Then
                    mailcontent = "The network connection RESUME " & Now() & ". Connected to (SSID) :" & use_ssid & ". Last success connection on : "
                    mailsubject = SID & "The network connection Resume " & Now()
                End If



                '  mailcontent = mailcontent & reader("lastupdate")

                sendnotification(email_from, email_lst, mailsubject, mailcontent)

            Else

                employeeInfo.username = MySQLGetUserInfo(CardNo, "empfullname") 'empfullname or name
                If Not employeeInfo.username = "" Then 'Card no is exist
                    ret = employeeInfo.inout = "OUT"
                    ret = employeeInfo.iptxt = SID
                    ret = inoutTxt(employeeInfo)      'Add inout text
                    ret = TodayCount(employeeInfo)    'Add notetext
                    ret = Insertinfo(employeeInfo)    'Insert data

                    If ret Then
                        sts = 0    'Insert data success
                    Else
                        sts = 1    'write failed
                    End If

                Else
                    sts = 2 'Card not found
                End If
            End If
            Response.Write("<sts>" & sts & "</sts>")
            Response.Write("<cardno>" & CardNo & "</cardno>")
            Response.Write("<username>" & employeeInfo.username & "</username>")
            Response.Write("<opt>" & opt & "</opt>")
        Catch ex As Exception
            Response.Write(ex.Message)
            sendnotification(email_from, email_lst, "Movement Time clock VB Exaption occur", ex.Message)
        End Try






    End Sub

End Class