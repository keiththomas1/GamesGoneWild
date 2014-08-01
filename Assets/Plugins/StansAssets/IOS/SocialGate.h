//
//  SocialGate.h
//  Unity-iPhone
//
//  Created by lacost on 2/15/14.
//
//

#import <Foundation/Foundation.h>
#import <Accounts/Accounts.h>
#import <Social/Social.h>

#if UNITY_VERSION < 450
#include "iPhone_View.h"
#endif



#include "ISNDataConvertor.h"

@interface SocialGate : NSObject
    + (id) sharedInstance;

- (void) twitterPost:(NSString*)status;
- (void) twitterPostWithMedia:(NSString*)status media: (NSString*) media;


- (void) fbPost:(NSString*)status;
- (void) fbPostWithMedia:(NSString*)status media: (NSString*) media;

- (void) mediaShare:(NSString*)text media: (NSString*) media;
@end
