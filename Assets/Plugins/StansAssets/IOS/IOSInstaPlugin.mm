//
//  IOSInstaPlugin.m
//  Unity-iPhone
//
//  Created by Osipov Stanislav on 3/8/14.
//
//

#import "IOSInstaPlugin.h"

@implementation IOSInstaPlugin

static IOSInstaPlugin *_sharedInstance;


+ (id)sharedInstance {
    
    if (_sharedInstance == nil)  {
        _sharedInstance = [[self alloc] init];
    }
    
    return _sharedInstance;
}
-(void) share:(NSString *)status media:(NSString *)media {
    NSLog(@"Insta share");
    
    NSData *imageData = [[NSData alloc] initWithBase64Encoding:media];
    UIImage *image = [[UIImage alloc] initWithData:imageData];
    
    
    
    if ([[[UIDevice currentDevice] systemVersion] floatValue] < 5.0) {
        float i = [[[UIDevice currentDevice] systemVersion] floatValue];
        NSString *str = [NSString stringWithFormat:@"We're sorry, but Instagram is not supported with your iOS %.1f version.", i];
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Message" message:str delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
        [alert show];
        
        UnitySendMessage("IOSInstagramManager", "OnPostFailed", [ISNDataConvertor NSStringToChar:@"3"]);

    } else {
        
        
        
        if ([MGInstagram isAppInstalled]) {
            UIViewController *vc =  UnityGetGLViewController();
            [MGInstagram postImage:image withCaption:status inView:vc.view delegate:self];
        } else {
            UnitySendMessage("IOSInstagramManager", "OnPostFailed", [ISNDataConvertor NSStringToChar:@"1"]);

        }
        
        
    }
    
}


- (void)documentInteractionControllerDidDismissOpenInMenu:(UIDocumentInteractionController *)controller {
    NSLog(@"documentInteractionControllerDidDismissOpenInMenu");
    UnitySendMessage("IOSInstagramManager", "OnPostFailed", [ISNDataConvertor NSStringToChar:@"2"]);
}


- (void) documentInteractionController: (UIDocumentInteractionController *) controller willBeginSendingToApplication: (NSString *) application {
     NSLog(@"willBeginSendingToApplication");
    UnitySendMessage("IOSInstagramManager", "OnPostSuccess", [ISNDataConvertor NSStringToChar:@""]);
}


extern "C" {
    
    
    void _instaShare(char* encodedMedia, char* text) {
        
        NSString *status = [ISNDataConvertor charToNSString:text];
        NSString *media = [ISNDataConvertor charToNSString:encodedMedia];
        
        [[IOSInstaPlugin sharedInstance] share:status media:media];
        
    }
    
    
    
    
}



@end

