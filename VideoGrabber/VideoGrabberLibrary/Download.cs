using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace VideoGrabberLibrary;

public class Download
{
    private YoutubeClient youtubeClient { get; set; }

    public Download()
    {
        youtubeClient = new YoutubeClient();
    }

    //string url = @"https://www.youtube.com/watch?v=W9vq4oOKYF8";

    public async Task GetVideo()
    {
        string outputDirectory = @$"C:\Users\{Environment.UserName}\Desktop\";
        List<string> videoUrl = new()
        {
            //@"https://www.youtube.com/watch?v=W9vq4oOKYF8",
            //@"https://www.youtube.com/watch?v=NH8mmayZ1jA",
            @"https://www.youtube.com/watch?v=9RIeycixkK8",
            @"https://www.youtube.com/watch?v=Ghd2bkIadG4",
            @"https://www.youtube.com/watch?v=KsthiOYDM9A",
            //@"https://www.youtube.com/watch?v=JISQMhtXiSM&sttick=1"
        };

        try
        {
            foreach (string url in videoUrl)
            {
                await GetMuxedVideo(url, outputDirectory, Container.WebM, ConversionPreset.UltraFast);
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"An error occured while downloading the videos: {ex.Message}");
        }
    }

    // first create function to get data and when button is clicked just then to create a dialog box and save the file so specific location
    private async Task<IVideoStreamInfo> GetYoutubeVideo(string url, bool download = false)
    {
        IVideoStreamInfo streamVideoInfo = null;

        try
        {
            Dictionary<string, string> videoInfo = await GetVideoInformation(url);
            StreamManifest streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(url);

            streamVideoInfo = streamManifest
                .GetVideoOnlyStreams()
                .Where(v => v.Container == Container.Mp4)
                // .First(s => s.VideoQuality.Label == "1080p60")
                .GetWithHighestVideoQuality();

            // Get stream
            foreach (var item in streamManifest.Streams)
            {
                // info about available records
                //Console.WriteLine(item);
            }

            //var stream = await youtube.Videos.Streams.GetAsync(streamVideoInfo);
            //Console.WriteLine(video.Description);
            //Console.WriteLine(await youtube.Videos.DownloadAsync(streamAudioInfo));

            // Download stream to a file
            //await youtube.Videos.Streams.DownloadAsync(streamAudioInfo, $"{outputDirectory}New.{streamAudioInfo.Container.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return streamVideoInfo;
    }

    private async Task<IStreamInfo> GetYoutubeAudio(string url, string outputDirectory, Container container, bool downloadAudio = false)
    {
        IStreamInfo streamAudioInfo = null;

        try
        {
            Dictionary<string, string> videoInfo = await GetVideoInformation(url);
            StreamManifest streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(url);

            if (downloadAudio == false)
            {
                streamAudioInfo = streamManifest
                    .GetAudioStreams()
                    .Where(a => a.Container == container)
                    .GetWithHighestBitrate();
            }
            else
            {
                await youtubeClient.Videos.Streams.DownloadAsync(streamAudioInfo, $"{outputDirectory}{videoInfo["title"]}.{container.Name}");
            }

            // streamAudioInfo.Container.Name

            // TODO: padaryti checkinika kokie conteineriai yra ir tik tada priimti pasirinkima
            // Arba kai idedi url tada su event atnaujinti ir parodyti visus available containers
            foreach (var item in streamManifest.Streams)
            {
                //Console.WriteLine(item.Container);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return streamAudioInfo;
    }

    private async Task<string> GetMuxedVideo(string url, string outputDirectory, Container containerType, ConversionPreset presetType)
    {
        string result = String.Empty;

        try
        {
            IStreamInfo audioRecord =  await GetYoutubeAudio(url, outputDirectory, Container.Mp4);
            IVideoStreamInfo videoRecord = await GetYoutubeVideo(url);
            IStreamInfo[] streamInformation = { audioRecord, videoRecord };

            Dictionary<string, string> videoInfo = await GetVideoInformation(url);

            string finalOutputPath = $"{outputDirectory}{videoInfo["title"]}.{containerType.Name}";
            string ffmpegPath = @$"C:\Users\{Environment.UserName}\Desktop\ffmpeg";

            await youtubeClient.Videos
                .DownloadAsync(streamInformation, new ConversionRequestBuilder(finalOutputPath)
                .SetContainer(containerType)
                .SetPreset(presetType)
                .SetFFmpegPath(ffmpegPath)
                .Build());

            result = "It worked!";
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
        }

        return result;
    }

    private async Task<Dictionary<string, string>> GetVideoInformation (string url)
    {
        Dictionary<string, string> videoInformation = new();

        try
        {
            Video video = await youtubeClient.Videos.GetAsync(url);
            
            string title = video.Title;
            string author = video.Author.ToString();
            string duration = video.Duration.ToString();
            string description = video.Description;

            // TODO: think how to clean the name without symbols

            videoInformation.Add("title", title);
            videoInformation.Add("author", author);
            videoInformation.Add("duration", duration);
            videoInformation.Add("description", description);
        }
        catch (Exception ex)
        {
            // loginima padaryti
            Console.Error.WriteLine(ex);
        }

        return videoInformation;
    }
}
