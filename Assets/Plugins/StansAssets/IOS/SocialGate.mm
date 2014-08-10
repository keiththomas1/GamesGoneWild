//
//  SocialGate.m
//  Unity-iPhone
//
//  Created by lacost on 2/15/14.
//
//

#import "SocialGate.h"


@implementation SocialGate

static SocialGate *_sharedInstance;


+ (id)sharedInstance {
    
    if (_sharedInstance == nil)  {
        _sharedInstance = [[self alloc] init];
    }
    
    return _sharedInstance;
}

-(void) mediaShare:(NSString *)text  media:(NSString *)media {
    UIActivityViewController *controller;
                                            
                                            
    if(media.length != 0) {
        NSData *imageData = [[NSData alloc] initWithBase64Encoding:media];
        UIImage *image = [[UIImage alloc] initWithData:imageData];
        
        if(text.length != 0) {
            controller = [[UIActivityViewController alloc] initWithActivityItems:@[text, image] applicationActivities:nil];
        } else {
            controller = [[UIActivityViewController alloc] initWithActivityItems:@[image] applicationActivities:nil];
        }
        
    } else {
        controller = [[UIActivityViewController alloc] initWithActivityItems:@[text] applicationActivities:nil];
    }
    
    
    UIViewController *vc =  UnityGetGLViewController();
    [vc presentViewController:controller animated:YES completion:nil];
    
}


-(void) twitterPostWithMedia:(NSString *)status media:(NSString *)media {
    NSLog(@"twitterPostWithMedia");
    
    [self mediaShare:@"" media:media];
    NSData *imageData = [[NSData alloc] initWithBase64Encoding:media];
    UIImage *image = [[UIImage alloc] initWithData:imageData];

    
    SLComposeViewController *tweetSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeTwitter];
    [tweetSheet setInitialText:status];
    [tweetSheet addImage:image];
    
    UIViewController *vc =  UnityGetGLViewController();
    

    
    [vc presentViewController:tweetSheet animated:YES completion:nil];
    
    tweetSheet.completionHandler = ^(SLComposeViewControllerResult result) {
        NSArray *vComp;
        switch(result) {
                //  This means the user cancelled without sending the Tweet
            case SLComposeViewControllerResultCancelled:
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [tweetSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                
                NSLog(@"Tweet message was cancelled");
                UnitySendMessage("IOSSocialManager", "OnTwitterPostFailed", [ISNDataConvertor NSStringToChar:@""]);
                break;
                //  This means the user hit 'Send'
            case SLComposeViewControllerResultDone:
                NSLog(@"Done pressed successfully");
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [tweetSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                UnitySendMessage("IOSSocialManager", "OnTwitterPostSuccess", [ISNDataConvertor NSStringToChar:@""]);
                break;
        }
    };
    
}

- (void) twitterPost:(NSString *)status {
    NSLog(@"twitterPost");
    
   
    SLComposeViewController *twSheet = [SLComposeViewController  composeViewControllerForServiceType:SLServiceTypeTwitter];
    [twSheet setInitialText:status];
    
    
    UIViewController *vc =  UnityGetGLViewController();
    
    [vc presentViewController:twSheet animated:YES completion:nil];
    
    twSheet.completionHandler = ^(SLComposeViewControllerResult result) {
        NSArray *vComp;
        switch(result) {
                //  This means the user cancelled without sending the Tweet
            case SLComposeViewControllerResultCancelled:
                
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [twSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                NSLog(@"Tweet message was cancelled");
                UnitySendMessage("IOSSocialManager", "OnTwitterPostFailed", [ISNDataConvertor NSStringToChar:@""]);
                break;
                //  This means the user hit 'Send'
            case SLComposeViewControllerResultDone:
                
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [twSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                NSLog(@"Done pressed successfully");
                UnitySendMessage("IOSSocialManager", "OnTwitterPostSuccess", [ISNDataConvertor NSStringToChar:@""]);
                break;
        }
    };
}


- (void) fbPost:(NSString *)status {
    
    NSLog(@"fbPost");
    
    SLComposeViewController *fbSheet = [SLComposeViewController  composeViewControllerForServiceType:SLServiceTypeFacebook];
    [fbSheet setInitialText:status];
    
    
    UIViewController *vc =  UnityGetGLViewController();
    
    [vc presentViewController:fbSheet animated:YES completion:nil];
    
    fbSheet.completionHandler = ^(SLComposeViewControllerResult result) {
        NSArray *vComp;
        switch(result) {
                //  This means the user cancelled without sending the Tweet
            case SLComposeViewControllerResultCancelled:
                
                
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [fbSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                NSLog(@"Facebook message was cancelled");
                UnitySendMessage("IOSSocialManager", "OnFacebookPostFailed", [ISNDataConvertor NSStringToChar:@""]);
                break;
                //  This means the user hit 'Send'
            case SLComposeViewControllerResultDone:
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [fbSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                NSLog(@"Facebook pressed successfully");
                UnitySendMessage("IOSSocialManager", "OnFacebookPostSuccess", [ISNDataConvertor NSStringToChar:@""]);
                break;
        }
    };
    
}




- (NSString*) photoFilePath {
    return [NSString stringWithFormat:@"%@/%@",[NSHomeDirectory() stringByAppendingPathComponent:@"Documents"],@"tempinstgramphoto.igo"];
}


-(void) fbPostWithMedia:(NSString *)status media:(NSString *)media {
    
    
    
    NSData *imageData = [[NSData alloc] initWithBase64Encoding:media];
    UIImage *image = [[UIImage alloc] initWithData:imageData];
    
    
    SLComposeViewController *fbSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeFacebook];
    [fbSheet setInitialText:status];
    [fbSheet addImage:image];
    
    UIViewController *vc =  UnityGetGLViewController();
    
    [vc presentViewController:fbSheet animated:YES completion:nil];
    
    fbSheet.completionHandler = ^(SLComposeViewControllerResult result) {
        NSArray *vComp;
        switch(result) {
                
            case SLComposeViewControllerResultCancelled:
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [fbSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                
                NSLog(@"Tweet message was cancelled");
                UnitySendMessage("IOSSocialManager", "OnFacebookPostFailed", [ISNDataConvertor NSStringToChar:@""]);
                break;
                //  This means the user hit 'Send'
            case SLComposeViewControllerResultDone:
                
                vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
                if ([[vComp objectAtIndex:0] intValue] < 7) {
                    [fbSheet dismissViewControllerAnimated:YES completion:nil];
                }
                
                
                NSLog(@"Done pressed successfully");
                UnitySendMessage("IOSSocialManager", "OnFacebookPostSuccess", [ISNDataConvertor NSStringToChar:@""]);
                break;
        }
        
        
        
    };
    
    
}


extern "C" {
    
    
    //--------------------------------------
	//  IOS Native Plugin Section
	//--------------------------------------
    
    
    void _ISN_TwPost(char* text) {
        NSString *status = [ISNDataConvertor charToNSString:text];
        [[SocialGate sharedInstance] twitterPost:status];
    }
    
    
    
    void _ISN_TwPostWithMedia(char* text, char* encodedMedia) {
        NSString *status = [ISNDataConvertor charToNSString:text];
        NSString *media = [ISNDataConvertor charToNSString:encodedMedia];
        
        [[SocialGate sharedInstance] twitterPostWithMedia:status media:media];
    }
    
    
    void _ISN_FbPost(char* text) {
        NSString *status = [ISNDataConvertor charToNSString:text];
        [[SocialGate sharedInstance] fbPost:status];
    }
    
    
    void _ISN_FbPostWithMedia(char* text, char* encodedMedia) {
        
        NSString *status = [ISNDataConvertor charToNSString:text];
        NSString *media = [ISNDataConvertor charToNSString:encodedMedia];
        
        [[SocialGate sharedInstance] fbPostWithMedia:status media:media];
    }
    
    void _ISN_MediaShare(char* text, char* encodedMedia) {
        NSString *status = [ISNDataConvertor charToNSString:text];
        NSString *media = [ISNDataConvertor charToNSString:encodedMedia];
        
        [[SocialGate sharedInstance] mediaShare:status media:media];
        
    }
    
    
    //--------------------------------------
	//  Mobile Social Plugin Section
	//--------------------------------------
    
    
    void _MSP_TwPost(char* text) {
        _ISN_TwPost(text);
    }
    
    
    void _MSP_TwPostWithMedia(char* text, char* encodedMedia) {
        _ISN_TwPostWithMedia(text, encodedMedia);
    }
    
    
    void _MSP_FbPost(char* text) {
        _ISN_FbPost(text);
    }
    
    
    void _MSP_FbPostWithMedia(char* text, char* encodedMedia) {
        _ISN_FbPostWithMedia(text, encodedMedia);
    }
    
    void _MSP_MediaShare(char* text, char* encodedMedia) {
        _ISN_MediaShare(text, encodedMedia);
    }
    
    
}



@end
