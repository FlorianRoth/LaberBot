
namespace LaberBot.WebService.Controller
{
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Web.Http;

    using LaberBot.Bot;

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlayController : ApiController
    {
        private readonly ILaberBot _bot;

        private readonly WebServiceOptions _options;

        private readonly IAudioPlayer _player;

        private readonly ISoundRepository _soundRepository;

        [ImportingConstructor]
        public PlayController(
            ILaberBot bot,
            WebServiceOptions options,
            IAudioPlayer player,
            ISoundRepository soundRepository)
        {
            _bot = bot;
            _options = options;
            _player = player;
            _soundRepository = soundRepository;
        }

        // GET api/play/<sound>/<user>
        [Route("api/play/{sound}/{user}")]
        public IHttpActionResult Get(string sound, string user)
        {
            var file = _soundRepository.GetSoundFile(sound);
            if (null == file)
            {
                return BadRequest($"Sound '{sound}' does not exist");
            }
            
            var server = _bot.FindServers(_options.Server).FirstOrDefault();
            if (null == server)
            {
                return BadRequest($"Could not find server '{_options.Server}'");
            }

            var discordUser = server.FindUsers(user).FirstOrDefault();
            if (discordUser == null)
            {
                return BadRequest($"Could not find user '{user}'");
            }

            var channel = discordUser.VoiceChannel;
            if (null == channel)
            {
                return BadRequest("Could not find voice channel");
            }
            
            _player.PlayAsync(channel, file.Path);

            return Ok();
        }
    }
}
