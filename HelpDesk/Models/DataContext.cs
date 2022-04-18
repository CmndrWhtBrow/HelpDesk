using HelpDesk.Configurations;
using HelpDesk.Models.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HelpDesk.Models
{
    public class DataContext : IDataContext
    {
        private readonly IOptionsMonitor<HelpDeskSettings> _helpDeskSettings;
        public DataContext(IOptionsMonitor<HelpDeskSettings> helpDeskSettings)
        {
            _helpDeskSettings = helpDeskSettings ?? throw new ArgumentNullException(nameof(helpDeskSettings));
            CurrentState = new DataState();
            InitDataContext();

        }
        public DataState CurrentState { get; set; }

        private void InitDataContext()
        {
            if (!File.Exists(_helpDeskSettings.CurrentValue.DataContextFileLocation))
            {
                File.Create(_helpDeskSettings.CurrentValue.DataContextFileLocation);
            }

            JsonSerializer? jsonSearlizer = new JsonSerializer();
            using StreamReader streamReader = File.OpenText(_helpDeskSettings.CurrentValue.DataContextFileLocation);
            using JsonReader jsonReader = new JsonTextReader(streamReader);

            CurrentState = jsonSearlizer.Deserialize<DataState>(jsonReader) ?? new DataState();
        }

        public void SaveState()
        {
            if (!File.Exists(_helpDeskSettings.CurrentValue.DataContextFileLocation))
            {
                File.Create(_helpDeskSettings.CurrentValue.DataContextFileLocation);
            }

            JsonSerializer? jsonSearlizer = new JsonSerializer();
            using StreamWriter streramWriter = new StreamWriter(_helpDeskSettings.CurrentValue.DataContextFileLocation);
            using JsonWriter jsonWriter = new JsonTextWriter(streramWriter);
            jsonSearlizer.Serialize(jsonWriter, CurrentState);

        }
    }
}
