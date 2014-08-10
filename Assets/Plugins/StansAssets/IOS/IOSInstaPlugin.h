//
//  IOSInstaPlugin.h
//  Unity-iPhone
//
//  Created by Osipov Stanislav on 3/8/14.
//
//

#import <Foundation/Foundation.h>
#if UNITY_VERSION < 450
#include "iPhone_View.h"
#endif
#include "MGInstagram.h"
#import "ISNDataConvertor.h"

@interface IOSInstaPlugin : NSObject<UIDocumentInteractionControllerDelegate>


+ (id) sharedInstance;

- (void) share:(NSString*)status media: (NSString*) media;


@end
