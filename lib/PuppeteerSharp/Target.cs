﻿using System;
using System.Threading.Tasks;

namespace PuppeteerSharp
{
    public class Target
    {
        private Browser _browser;
        private TargetInfo _targetInfo;
        private bool _isInitialized;

        public Target(Browser browser, TargetInfo targetInfo)
        {
            _browser = browser;
            _targetInfo = targetInfo;

            _isInitialized = _targetInfo.Type != "page" || _targetInfo.Url != string.Empty;

            if (_isInitialized)
            {
                Initialized();
            }
        }

        public string Url => _targetInfo.Url;
        public string Type => _targetInfo.Type == "page" || _targetInfo.Type == "service_worker" ? _targetInfo.Type : "other";

        public async Task<Page> Page()
        {
            if (_targetInfo.Type == "page")
            {
                var client = await _browser.Connection.CreateSession(_targetInfo.targetId);
                await PuppeteerSharp.Page.CreateAsync(client, _browser.IgnoreHTTPSErrors, _browser.AppMode);
            }

            return null;
        }

        public void TargetInfoChanged(TargetInfo targetInfo) 
        {
            var previousUrl = _targetInfo.Url;
            _targetInfo = targetInfo;

            if (!_isInitialized && (_targetInfo.Type != "page" || _targetInfo.Url != string.Empty))
            {
                _isInitialized = true;
                Initialized();
            }

            if (previousUrl != targetInfo.Url)
            {
                _browser.ChangeTarget(targetInfo);
            }
        }

        private void Initialized()
        {
            
        }
    }
}
