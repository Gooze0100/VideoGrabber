using YoutubeExplode;
using YoutubeExplode.Converter;

namespace VideoGrabberLibrary;

public class Download
{

    //string url = @"https://www.youtube.com/watch?v=W9vq4oOKYF8";

    public async Task GetVideo()
    {
        string outputDirectory = @"C:\Users\kalnigi\Desktop";
        List<string> videoUrl = new()
        {
            @"https://www.youtube.com/watch?v=W9vq4oOKYF8",
            @"https://www.youtube.com/watch?v=W9vq4oOKYF8"
        };

        try
        {
            foreach (string url in videoUrl)
            {
                await YoutubeVideo(url, outputDirectory);
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"An error occured while downloading the videos: {ex.Message}");
        }
    }

    private async Task YoutubeVideo(string url, string outputDirectory)
    {
        YoutubeClient youtube = new();
        var video = await youtube.Videos.GetAsync(url);

        string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        var muxedStreams = streamManifest.GetMuxedStreams().OrderByDescending(s => s.VideoQuality).ToList();

        if (muxedStreams.Any())
        {
            var streamInfo = muxedStreams.First();
            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(streamInfo.Url);
            var dateTime = DateTime.Now;

            string outputFilePath = Path.Combine(outputDirectory, $"{sanitizedTitle}.{streamInfo.Container}");
            using var outputStream = File.Create(outputFilePath);
            await stream.CopyToAsync(outputStream);

            Console.WriteLine("Download completed!");
            Console.WriteLine($"Video saved as: {outputFilePath} - {dateTime}");
        }
        else
        {
            Console.WriteLine($"Not suitable video stream found for {video.Title}");
        }
    }
}
