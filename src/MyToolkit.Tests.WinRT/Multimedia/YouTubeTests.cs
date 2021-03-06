﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyToolkit.Multimedia;
using MyToolkit.Networking;

namespace MyToolkit.Tests.WinRT.Multimedia
{
    [TestClass]
    public class YouTubeTests
    {
        [TestMethod]
        public async Task When_loading_PQVlW4xbNuI_then_correct_uris_are_returned()
        {
            //// Arrange
            var youTubeId = "PQVlW4xbNuI";

            //// Act
            var allUris = await YouTube.GetUrisAsync(youTubeId);
            var uri720P = await YouTube.GetVideoUriAsync(youTubeId, YouTubeQuality.Quality480P, YouTubeQuality.Quality720P);
            var has480P = false; 
            try
            {
                var uri480P = await YouTube.GetVideoUriAsync(youTubeId, YouTubeQuality.Quality480P, YouTubeQuality.Quality480P);
                has480P = true; 
            }
            catch (Exception) { }

            //// Assert
            Assert.IsTrue(allUris.Any(u => u.VideoQuality == YouTubeQuality.Quality480P && !u.HasAudio));
            Assert.IsFalse(allUris.Any(u => u.VideoQuality == YouTubeQuality.Quality480P && u.HasAudio));
            Assert.IsFalse(has480P); //// No 480p stream with audio available

            Assert.IsNotNull(uri720P); //// 720p stream with audio available
            Assert.AreEqual(YouTubeQuality.Quality720P, uri720P.VideoQuality);
        }

        [DataTestMethod]
        //[DataRow("7M_jHUawccI")] // rtmpe 
        [DataRow("kYQ8w7kxdDk")]
        [DataRow("J3UjJ4wKLkg")]
        [DataRow("O-zpOMYRi0w")]
        [DataRow("kPsd2XaBKzc")]
        [DataRow("Otx7FEINUds")]
        [DataRow("TGtWWb9emYI")]
        [DataRow("7-7knsP2n5w")]
        [DataRow("kPsd2XaBKzc")]
        [DataRow("Z6FPJOgfCkc")]
        [DataRow("xE2MxCv5vVY")]
        [DataRow("-ynZN3p6FH8")]
        [DataRow("JF8BRvqGCNs")]
        [DataRow("ohfSspPiOF8")]
        [DataRow("0G3_kG5FFfQ")]
        [DataRow("e-ORhEE9VVg")]
        [DataRow("nfWlot6h_JM")] // vevo 
        [DataRow("xFfCG_hffPA")] // vevo 
        [DataRow("H74AIvjDifo")] // vevo 
        //[DataRow("rSy0JtBCZMQ")] // different problem => missing URL key
        public async Task When_loading_video_then_uri_should_be_available_accessible(string youTubeId)
        {
            //// Act
            Debug.WriteLine(youTubeId);
            var uri = await YouTube.GetVideoUriAsync(youTubeId, YouTubeQuality.Quality360P, YouTubeQuality.Quality2160P);

            try
            {
                var request = new HttpGetRequest(uri.Uri);
                request.Timeout = TimeSpan.FromSeconds(1);

                var cancellationToken = new CancellationTokenSource(new TimeSpan(0, 0, 2));
                await Http.GetAsync(uri.Uri, cancellationToken.Token);
            }
            catch (OperationCanceledException exception)
            {
                Assert.IsNotNull(exception);
            }

            //// Assert
            Assert.IsNotNull(uri);
            Assert.IsTrue(uri.HasAudio);
            Assert.IsTrue(uri.HasVideo);
        }

        [DataTestMethod]
        [DataRow("kYQ8w7kxdDk")]
        public async Task When_loading_4k_video_then_uri_it_should_be_available(string youTubeId)
        {
            //// Act
            var allUris = await YouTube.GetUrisAsync(youTubeId);

            //// Assert
            Assert.IsTrue(allUris.Any(u => u.VideoQuality == YouTubeQuality.Quality2160P));
        }
    }
}
