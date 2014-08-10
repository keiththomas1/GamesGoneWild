//
//  Unity3d.h
//
//  Created by Osipov Stanislav on 1/11/13.
//
//

#import <Foundation/Foundation.h>

@interface ISNDataConvertor : NSObject

+ (NSString*) charToNSString: (char*)value;
+ (const char *) NSIntToChar: (NSInteger) value;
+ (const char *) NSStringToChar: (NSString *) value;

@end

