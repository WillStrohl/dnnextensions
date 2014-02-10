Imports WillStrohl.API.oEmbed

Module Module1

    Sub Main()

        Console.WriteLine("*** BEGIN TEST ***")
        Console.WriteLine(Environment.NewLine)

        'Try

        'Dim strUrl As String = "http://www.flickr.com/photos/willstrohl/5475370036/"
        'Dim strUrl As String = "http://www.flickr.com/photos/willstrohl/5445567810/in/set-72157625922948633/"
        'Dim strUrl As String = "http://my.opera.com/Verhovensky/albums/showpic.dml?album=6391941&picture=97423861"
        'Dim strUrl As String = "http://xkcd.com/150/"
        Dim strUrl As String = "http://www.amazon.com/gp/product/B001P7G0ZQ/ref=s9_bbs_gw_d1_ir03?pf_rd_m=ATVPDKIKX0DER&pf_rd_s=center-2&pf_rd_r=0SNXV001BY90D6SZSJGE&pf_rd_t=101&pf_rd_p=470938631&pf_rd_i=507846"
        'Dim strUrl As String = "http://www.hulu.com/watch/42709/family-guy-peters-new-job"
        'Dim strUrl As String = "http://qik.com/video/4132968"
        'Dim strUrl As String = "http://revision3.com/geekbeattv/2011-03-17"
        'Dim strUrl As String = "http://www.viddler.com/explore/homedepotvideo/videos/77/"
        'Dim strUrl As String = "http://www.vimeo.com/21003140"
        'Dim strUrl As String = "http://www.youtube.com/watch?v=CArWivYt208"

        Dim ctlOEmbed As New WillStrohl.API.oEmbed.Wrapper
        Dim strResult = ctlOEmbed.GetContent(New RequestInfo(strUrl))
        'Dim strResult = ctlOEmbed.GetContent(New RequestInfo(strUrl, 500, 350)) ' viddler, flickr

        Console.WriteLine(Environment.NewLine)
        Console.WriteLine(strResult)

        'Catch ex As Exception

        '    Console.WriteLine(Environment.NewLine)
        '    Console.WriteLine(ex.Message)

        'End Try

        Console.WriteLine(Environment.NewLine)
        Console.WriteLine("*** END TEST ***")
        Console.WriteLine(Environment.NewLine)
        Console.WriteLine("Press <ENTER> to Continue")
        Console.ReadLine()

    End Sub

End Module
