// Web view integration plug-in for Unity iOS.

#import <Foundation/Foundation.h>
#import "GAI.h"
#import "GAIDictionaryBuilder.h"

//NSString * const kGTLAuthScopePlusLogin = @"https://www.googleapis.com/auth/plus.login";

static NSString * gAppID= @"ID";
#pragma mark Plug-in Functions

extern "C" {
    void GoogleAnalytics_startSession(const char* tracking_id);
    void GoogleAnalytics_logEventWithParameters(const char* category, const char* action,const char* label, const char* value);
}

void GoogleAnalytics_startSession(const char* tracking_id)
{
    gAppID = [NSString stringWithUTF8String:tracking_id];
    [[GAI sharedInstance] trackerWithTrackingId:gAppID];
}

void GoogleAnalytics_logEvent(const char* category, const char* action,const char* label, const char* value) {
    id<GAITracker> tracker = [[GAI sharedInstance] defaultTracker];
    
    if(tracker){
        NSNumber *valueNumber = [NSNumber numberWithInt:[[NSString stringWithUTF8String:value] integerValue]];
        GAIDictionaryBuilder *builder =  [GAIDictionaryBuilder createEventWithCategory:[NSString stringWithUTF8String:category]   // Event category (required)
                                                                                action:[NSString stringWithUTF8String:action] // Event action (required)
                                                                                 label:[NSString stringWithUTF8String:label]         // Event label
                                                                                 value:valueNumber];
        [tracker send:[builder build]];
    }
}
