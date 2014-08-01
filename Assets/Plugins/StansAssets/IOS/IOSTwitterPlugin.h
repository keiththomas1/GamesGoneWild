//
//  IOSNative.h
//
//  Created by Osipov Stanislav on 1/11/13.
//
//

#import <Foundation/Foundation.h>
#if UNITY_VERSION < 450
#include "iPhone_View.h"
#endif
#import "ISNDataConvertor.h"

#import <Accounts/Accounts.h>
#import <Social/Social.h>

@interface IOSTwitterPlugin : NSObject

+ (id) sharedInstance;

- (void) initTwitterPlugin;
- (void) authificateUser;
- (void) loadUserData;
- (void) post:(NSString*)status;
- (void) postWithMedia:(NSString*)status media: (NSString*) media;


@end
