//
//  GameView.h
//  GwentGame
//
//  Created by Adrian on 18/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "Controlador.h"
NS_ASSUME_NONNULL_BEGIN

@interface GameView : NSView{
    IBOutlet __weak Controlador *controlador;
    NSString *backgroundName;
}

@property (nonatomic, copy) NSString *backgroundName;

@end



NS_ASSUME_NONNULL_END
